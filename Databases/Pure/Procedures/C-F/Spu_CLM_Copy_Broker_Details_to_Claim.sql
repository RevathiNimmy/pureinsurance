SET Quoted_IdentIfier  Off
GO
SET Ansi_Nulls  ON
GO
EXECUTE ddlDropProcedure 'Spu_CLM_Copy_Broker_Details_to_Claim'
GO

Create procedure Spu_CLM_Copy_Broker_Details_to_Claim
@Claim_Ri_Arrangement_id int
AS
BEGIN
	DECLARE @Claim_RiLineID int,
		@Policy_RiLineID int,
		@PartyCnt int,
		@stype varchar(2),
		@Part_percent float,
		@Party_Cnt int,
		@GroupingId int,
		@Claim_RILinePriority int
	
	DECLARE FX_Copy_Broker CURSOR FAST_FORWARD FOR
		Select c_ral.ri_arrangement_line_id,c_ral.party_cnt,c_ral.type,ral.ri_arrangement_line_id 
			From claim_ri_arrangement_line c_ral WITH (NOLOCK)
			Inner Join ri_arrangement_line ral WITH (NOLOCK) on  c_ral.party_cnt=ral.party_cnt and ral.grouping=c_ral.grouping 
			Where c_ral.ri_arrangement_id=@Claim_Ri_Arrangement_id
			And c_ral.type = 'FX'  
			Order by c_ral.ri_arrangement_line_id

	DECLARE F_Copy_Broker CURSOR FAST_FORWARD FOR
		Select c_ral.ri_arrangement_line_id,c_ral.party_cnt,c_ral.type,c_ral.priority 
			From claim_ri_arrangement_line c_ral WITH (NOLOCK)
			Where c_ral.ri_arrangement_id=@Claim_Ri_Arrangement_id
			And c_ral.type = 'F'  
			Order by c_ral.ri_arrangement_line_id

	DECLARE Upd_GroupingID CURSOR FAST_FORWARD FOR
		Select distinct grouping from claim_ri_arrangement_line WITH (NOLOCK) 
			Where ri_arrangement_id=@Claim_Ri_Arrangement_id 
			And grouping is not null 

--Copy Brokers for FAC PROP if any
	OPEN F_Copy_Broker
	
	FETCH NEXT FROM F_Copy_Broker
		INTO @Claim_RiLineID,@PartyCnt,@stype,@Claim_RILinePriority
		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			IF EXISTS(Select * from party_insurer where is_ri_broker=1 and party_cnt=@PartyCnt)			
			BEGIN
					DECLARE Brokers_FACPROP CURSOR FAST_FORWARD FOR
						Select ri_party_cnt,participation_percent From 
							Ri_Arrangement_line_Broker_Participants 
							Where ri_arrangement_line_id=(
							Select ri_arrangement_line_id 
							From ri_arrangement_line WITH (NOLOCK) 
							Where ri_arrangement_id =(Select ra.ri_arrangement_id from ri_arrangement ra WITH (NOLOCK), Claim_ri_Arrangement cra WITH (NOLOCK)
										  Where ra.risk_cnt=cra.risk_cnt 
										  And ra.ri_band_id=ra.ri_band_id 
										  And ra.ri_model_id=cra.ri_model_id
										  And cra.ri_arrangement_id=@Claim_Ri_Arrangement_id 
										  --And ra.is_modified=1)
										  And ra.original_flag=0) 
							And type ='F'
							And Party_cnt=@PartyCnt
							And Priority=@Claim_RILinePriority)

					OPEN Brokers_FACPROP	
				
					FETCH NEXT FROM Brokers_FACPROP
						INTO @Party_Cnt,@Part_percent

					WHILE @@FETCH_STATUS = 0
					BEGIN

						Insert Into Claim_Ri_Arrangement_line_Broker_Participants(claim_ri_arrangement_line_id,
									ri_party_cnt,
									participation_percent)
						Values (@Claim_RiLineID,
								@Party_Cnt,
								@Part_percent)
							
						FETCH NEXT FROM Brokers_FACPROP
							INTO @Party_Cnt,@Part_percent
					END
					CLOSE Brokers_FACPROP
					DEALLOCATE Brokers_FACPROP
			END				

			FETCH NEXT FROM F_Copy_Broker
			INTO @Claim_RiLineID,@PartyCnt,@stype,@Claim_RILinePriority
		END	

		CLOSE F_Copy_Broker
		DEALLOCATE F_Copy_Broker

--Add FAX Brokers

	OPEN FX_Copy_Broker 
	
	FETCH NEXT FROM FX_Copy_Broker
		INTO @Claim_RiLineID,@PartyCnt,@stype,@Policy_RiLineID
		
		WHILE @@FETCH_STATUS = 0
		BEGIN

			DECLARE Brokers CURSOR FAST_FORWARD FOR
			Select  ri_party_cnt,participation_percent From 
				Ri_Arrangement_line_Broker_Participants WITH (NOLOCK) 
				Where ri_arrangement_line_id=@Policy_RiLineID

			OPEN Brokers

				FETCH NEXT FROM Brokers
				INTO @Party_Cnt,@Part_percent

					WHILE @@FETCH_STATUS = 0
					BEGIN

						Insert Into Claim_Ri_Arrangement_line_Broker_Participants(claim_ri_arrangement_line_id,
									ri_party_cnt,
									participation_percent)
						Values (@Claim_RiLineID,
								@Party_Cnt,
								@Part_percent)
							
						FETCH NEXT FROM Brokers
							INTO @Party_Cnt,@Part_percent
					END
					CLOSE Brokers
					DEALLOCATE Brokers
			
			FETCH NEXT FROM FX_Copy_Broker
			INTO @Claim_RiLineID,@PartyCnt,@stype,@Policy_RiLineID

		END
		CLOSE FX_Copy_Broker
		
		DEALLOCATE FX_Copy_Broker

--Update GroupingID
	OPEN Upd_GroupingID

			FETCH NEXT FROM Upd_GroupingID
			INTO @GroupingID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				Update claim_ri_arrangement_line Set Grouping = (
						Select min(ri_arrangement_line_id) 
						From claim_ri_arrangement_line WITH (NOLOCK) 
						Where ri_arrangement_id=@Claim_Ri_Arrangement_id
						And grouping = @GroupingID	)
						Where grouping = @GroupingID
						And ri_arrangement_id=@Claim_Ri_Arrangement_id

			FETCH NEXT FROM Upd_GroupingID
			INTO @GroupingID

			END
	CLOSE Upd_GroupingID
	DEALLOCATE Upd_GroupingID
END  
GO
