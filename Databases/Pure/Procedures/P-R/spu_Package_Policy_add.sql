SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Package_Policy_add'
GO

CREATE PROCEDURE spu_Package_Policy_add
    @PackageHeaderId integer, 
    @InsuranceFileCnt integer,
    @IsDeleted tinyint,
    @EffectiveDate datetime,
    @Package_Policy_Id integer OUTPUT
AS

    INSERT INTO Package_Policy
        (
            Package_Header_Id,
            Insurance_File_cnt,
            Is_deleted,
            Effective_Date,
            Modified_Date
        )
        VALUES
        (
            @PackageHeaderId,
            @InsuranceFileCnt,
            @IsDeleted,
            @EffectiveDate,
            GetDate()
        )

select @Package_Policy_Id=Package_Policy_Id from Package_Policy 
    where 
        Insurance_File_Cnt=@InsuranceFileCnt and
        Package_Header_Id=@PackageHeaderId and 
        Is_Deleted=0 
GO

