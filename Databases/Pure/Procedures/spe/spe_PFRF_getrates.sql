SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFRF_getrates'
GO
CREATE PROCEDURE spe_PFRF_getrates
    @companyno int,
    @schemeno int,
    @schemeversion int,
    @startdate datetime,
    @productfamily char(1)
AS
    SELECT
        enddate, daysdelay, depositreq,
        depositpc, allowprotection, protectrate, mininterest,
        min1, max1, rate1, apr1, R1Com, APR1Com, Com1PC,
        min2, max2, rate2, apr2, R3Com, APR2Com, Com2PC,
        min3, max3, rate3, apr3, R3Com, APR3Com, Com3PC,
        min4, max4, rate4, apr4, R4Com, APR4Com, Com4PC,
        min5, max5, rate5, apr5, R5Com, APR5Com, Com5PC
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

