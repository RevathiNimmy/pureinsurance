SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_drivers'
GO


CREATE PROCEDURE spu_wp_drivers
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


DECLARE @name VARCHAR(60),
    @date_of_birth DATETIME,

    @gender VARCHAR(70),
    @type_of_licence VARCHAR(70),
    @date_passed_test DATETIME

    SELECT  @name = name,
        @date_of_birth = date_of_birth,
        @gender = gender,
        @type_of_licence = type_of_licence,
        @date_passed_test = date_passed_test
    FROM    drivers
    WHERE   insurance_file_cnt = @InsuranceFileCnt
    AND driver_number = @Instance1

    SELECT  'name' = @name,
        'date_of_birth' = @date_of_birth,
        'gender' = @gender,
        'type_of_licence' = @type_of_licence,
        'date_passed_test' = @date_passed_test
GO


