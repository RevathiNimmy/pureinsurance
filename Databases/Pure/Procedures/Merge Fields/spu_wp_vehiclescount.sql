SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_vehiclescount'
GO


CREATE PROCEDURE spu_wp_vehiclescount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  count(vehicle_number) 'how_many'
    FROM    vehicles
    WHERE   insurance_file_cnt = @InsuranceFileCnt
GO


