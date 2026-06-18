SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Package_Header_upd'
GO

CREATE PROCEDURE spu_Package_Header_upd
    @PartyCnt integer,
    @PackageCode char(10),
    @PackageDesc varchar(255),
    @PolicyStartDate	datetime,
    @PolicyEndDate datetime,
    @PolicyEffectiveDate datetime,
    @PolicyHolderName varchar(255),
    @AccountHandlerCnt  integer,
    @AgentCnt integer,
    @PolicyBranch integer,
    @PaymentMethod varchar(60),
    @Frequency integer,
    @PackageNotes text,
    @IsDeleted integer,
    @EffectiveDate datetime,
    @PackageCnt integer 
AS

    UPDATE Package_Header 
        SET
            Party_Cnt=@PartyCnt,
            Package_Code=@PackageCode	 ,	
            Package_Description=@PackageDesc,
            Policy_Start_Date=@PolicyStartDate,
            Policy_End_Date=@PolicyEndDate	,	
            Policy_Effective_Date=@PolicyEffectiveDate,
            Policy_Holder_Name=@PolicyHolderName,
            Policy_Handler_cnt=@AccountHandlerCnt,	
            Policy_Agent_cnt=@AgentCnt,	
            Policy_Branch=@PolicyBranch 	,	
            Policy_Payment_Method=@PaymentMethod,
            Policy_Renewal_Frequency=@Frequency,
            Package_Notes=@PackageNotes	,
            Is_deleted=@IsDeleted	,
            Effective_Date=@EffectiveDate
        WHERE Package_Header_Id=@PackageCnt

GO