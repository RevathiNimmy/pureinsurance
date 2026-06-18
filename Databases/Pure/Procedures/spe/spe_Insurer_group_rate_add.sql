SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Insurer_group_rate_add'
GO

CREATE PROCEDURE spe_Insurer_group_rate_add
    @party_cnt int,
    @Scheme int,
    @risk_group_id int,
    @effective_date datetime,
    @rate1 numeric(7,4),
    @value1 numeric(19,4),
    @minimum_total1 numeric(19,4),
    @rate2 numeric(7,4),

    @value2 numeric(19,4),
    @minimum_total2 numeric(19,4),
    @rate3 numeric(7,4),
    @value3 numeric(19,4),
    @minimum_total3 numeric(19,4)
AS
BEGIN
INSERT INTO Insurer_group_rate (
    party_cnt ,
    Scheme ,
    risk_group_id ,
    effective_date ,
    rate1 ,
    value1 ,
    minimum_total1 ,
    rate2 ,
    value2 ,
    minimum_total2 ,
    rate3 ,
    value3 ,
    minimum_total3 )
VALUES (
    @party_cnt,
    @Scheme,
    @risk_group_id,
    @effective_date,
    @rate1,
    @value1,
    @minimum_total1,
    @rate2,
    @value2,
    @minimum_total2,
    @rate3,
    @value3,
    @minimum_total3)
END

GO

