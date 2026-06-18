ddldropprocedure spu_get_claim_info_ex
go

--This SP is called by ClaimsDetails in bGISPMUExtras
--It is used in one of four modes to extract the required data.
--The complete array is constructed in ClaimsDetails
--The full definition of the constructed array is defined in "Best Practise Guide For Rule Delevopment"

-- ----------------------------------------------------------------------------------------------------
-- |                         Parameters                          |Mode Description                    |
-- ----------------------------------------------------------------------------------------------------
-- |Mode  |     ID              |claim_year_to_check|underwriting|                                    |
-- ----------------------------------------------------------------------------------------------------
-- |1     |Insurance_file_cnt   |The value indicates|            |Get basic claim details,            |
-- |      |                     |which year to use  |            |                                    |
-- |      |                     |1=current year	    |            |                                    |
-- |      |                     |2= previous year   |            |                                    |
-- |      |                     |3= 2 years ago     |            |                                    |
-- |      |                     |etc etc            |N/A         |                                    |
-- ----------------------------------------------------------------------------------------------------
-- |2     |Claim_id             |N/A                |N/A         |Get claim user defined fields       |
-- ----------------------------------------------------------------------------------------------------
-- |3     |Claim_id             |N/A                |1 or 0      |Get claim perils                    |
-- ----------------------------------------------------------------------------------------------------
-- |4     |Claim_peril_id       |N/A                |N/A         |Get claim peril user defined fields |
-- ----------------------------------------------------------------------------------------------- ----


CREATE PROCEDURE spu_get_claim_info_ex
    @mode INT,
    @ID integer,
    @underwriting INT = 0,
    @claim_year_to_check INT = 0
AS


if @mode =1  -- get basic claim details, @ID is insurance_file 
BEGIN

	DECLARE @start_date as datetime

	-- get the start date from the latest real policy (not MTA)
	set @start_date = 
	    (SELECT top 1 ifi2.cover_start_date
	        FROM    insurance_file ifi, insurance_file ifi2
		WHERE   ifi.insurance_file_cnt = @ID
		AND ifi2.insurance_file_type_id=2 --policy 
		AND ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt
		ORDER BY ifi2.policy_version DESC)

        -- get the claims for policies whose cover start dates are withing the search period
	SELECT
		claim_number,loss_from_date ,
		0 as amount,c.description,
		ps.description as status ,
		pc.description as [primary cause] ,
		sc.description as [secondary cause]  ,
		cc.description as catastrophe ,
		last_modified_date,
		NULL as [user defined fields],
		NULL as perils,
		claim_id
		from claim as c
		inner join progress_status as ps on ps.progress_status_id=c.claim_status_id
		inner join primary_cause as pc on pc.primary_cause_id=c.primary_cause_id
		left outer join secondary_cause as sc on sc.secondary_cause_id=c.secondary_cause_id
		left outer join catastrophe_code as cc on cc.catastrophe_code_id=c.catastrophe_code_id
		where policy_id in
		(
		        --get the policies whose cover start dates are withing the search period
			select ifi2.insurance_file_cnt
				FROM    insurance_file ifi,
				insurance_file ifi2
				inner join progress_status as ps on ps.progress_status_id=c.claim_status_id
				inner join primary_cause as pc on pc.primary_cause_id=c.primary_cause_id
				left outer join secondary_cause as sc on sc.secondary_cause_id=c.secondary_cause_id
				left outer join catastrophe_code as cc on cc.catastrophe_code_id=c.catastrophe_code_id
				WHERE   ifi.insurance_file_cnt = @ID
				AND     ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt
				AND     ifi2.cover_start_date >= dateadd(year,1-@claim_year_to_check,@start_date)
				AND     ifi2.cover_start_date <= dateadd(day,-1,dateadd(year,2-@claim_year_to_check,@start_date))
				AND     ifi2.insurance_file_cnt = c.policy_id
				AND     c.primary_cause_id NOT IN
				(
				        --this disregards any claims for this product type which are in the Product_Allowed_Causation table
					SELECT primary_cause_id
					FROM    Product_Allowed_Causation pac
					WHERE   ifi2.product_id = pac.product_id
				)
		)

END 		

if @mode =2  -- get claim user defined fields, @ID is claim ID
BEGIN
	SELECT 
	RDD.description,caption,
	value from Claim_User_Defined_Risk_Data as CUDRD 
	inner join risk_data_definition as RDD on CUDRD.risk_data_defn_id=RDD.risk_data_defn_id 
	where claim_id=@ID
END 

if @mode =3  -- get claim perils, @ID is claim ID
BEGIN
     IF @underwriting = 1 
        BEGIN
			SELECT 
    	    Max(cp.Description) As Description,
			sum(initial_reserve+revised_reserve) as revised_reserve,
			sum(paid_to_date) as paid_to_date  ,
			NULL as user_defined_fields, 
			cp.claim_peril_id,
			cp.sum_insured,
			NULL as recoveries,
			PT.Code As PerilCode
			from claim_peril cp 
			left outer join reserve as r on cp.claim_peril_id=r.claim_peril_id
			Left Join Peril_Type PT On PT.peril_type_id=Cp.Peril_type_id
			inner join claim c on c.claim_id=cp.claim_id
			where c.claim_id=@ID
			group by  cp.claim_peril_id,PT.Code,cp.sum_insured
        END
      ELSE
        BEGIN
			SELECT 
			Max(cp.Description) As Description,
			sum(revised_reserve) as revised_reserve,
			sum(paid_to_date) as paid_to_date  ,
			NULL as user_defined_fields, 
			cp.claim_peril_id,
	       PT.Code As PerilCode
		   from claim_peril cp 
			Left outer join reserve as r on cp.claim_peril_id=r.claim_peril_id
			Left Join Peril_Type PT On PT.peril_type_id=Cp.Peril_type_id
			inner join claim c on c.claim_id=cp.claim_id
			where c.claim_id=@ID
			group by  cp.claim_peril_id,PT.Code
        END

END 

if  @mode=4 -- get claim peril user defined fileds, @ID is claim_peril_id
BEGIN
	SELECT  
	peril_data_definition.description,  
	peril_data_definition.caption,  
	user_defined_peril_data.value
	FROM claim_peril
	inner join Peril_data_definition on Peril_data_definition.peril_type_id = claim_peril.peril_type_id  
	inner join user_defined_peril_data on user_defined_peril_data.peril_data_defn_id = peril_data_definition.peril_data_defn_id  
	WHERE claim_peril.claim_peril_id=@ID
END
