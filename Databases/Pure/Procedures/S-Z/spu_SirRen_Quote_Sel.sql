SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Quote_Sel'
GO


CREATE PROCEDURE spu_SirRen_Quote_Sel
    @effective_date datetime
AS


BEGIN

    SELECT
    rc.insurance_folder_cnt,
    rc.party_cnt,
    rc.renewal_date,
    rc.risk_code_id
    FROM Renewal_Control rc
    INNER JOIN Renewal_Settings rsr ON rsr.product_id = rc.risk_code_id
    WHERE rc.renewal_date BETWEEN @effective_date
    AND DATEADD(d, rsr.selection_day_num, @effective_date)
    AND rc.renewal_status_type_id = (SELECT renewal_status_type_id FROM renewal_status_type WHERE code = 'PRERENSEL')

END
GO


