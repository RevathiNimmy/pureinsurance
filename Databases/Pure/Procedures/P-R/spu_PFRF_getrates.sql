EXECUTE DDLDropProcedure 'spu_PFRF_getrates'

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFRF_getrates
    @companyno int,
    @schemeno int,
    @schemeversion int,
    @startdate datetime,
    @productfamily char(3)
AS
    SELECT
        enddate, daysdelay, depositreq,
        depositpc, allowprotection, protectrate, mininterest,
        min1, max1, rate1, R1Com,
        min2, max2, rate2, R3Com,
        min3, max3, rate3, R3Com, 
        min4, max4, rate4, R4Com, 
        min5, max5, rate5, R5Com
    FROM
        PFRF
    WHERE
        companyno = @companyno
    AND
        schemeno = @schemeno
    AND
        schemeversion = @schemeversion
    AND
        startdate < = @startdate
    AND
        enddate > = @startdate
    AND
        productfamily = @productfamily
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

