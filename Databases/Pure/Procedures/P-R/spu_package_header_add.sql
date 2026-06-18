SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Package_Header_add'
GO

CREATE PROCEDURE spu_Package_Header_add
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
    @IsDeleted tinyint,
    @EffectiveDate datetime,
    @Package_Header_Id integer OUTPUT
AS

    INSERT INTO Package_Header 
        (
            Party_Cnt,
            Package_Code	 ,	
            Package_Description,
            Policy_Start_Date,
            Policy_End_Date	,	
            Policy_Effective_Date,
            Policy_Holder_Name,
            Policy_Handler_cnt,	
            Policy_Agent_cnt,	
            Policy_Branch	,	
            Policy_Payment_Method,
            Policy_Renewal_Frequency,
            Package_Notes	,
            Is_deleted	,
            Effective_Date	,
            Modified_Date	
        )
        VALUES
        (
            @PartyCnt ,
            @PackageCode,
            @PackageDesc ,
            @PolicyStartDate,
            @PolicyEndDate ,
            @PolicyEffectiveDate ,
            @PolicyHolderName,
            @AccountHandlerCnt ,
            @AgentCnt ,
            @PolicyBranch  ,
            @PaymentMethod ,
            @Frequency ,
            @PackageNotes ,
            @IsDeleted,
	    @EffectiveDate,
            GetDate()
        )

select @Package_Header_Id=Package_Header_Id from Package_Header where 
	Party_cnt=@Partycnt and
        package_code=@PackageCode and 
        Is_Deleted=0 
GO
