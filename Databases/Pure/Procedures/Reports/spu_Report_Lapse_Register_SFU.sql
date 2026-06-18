
EXECUTE DDLDropProcedure 'spu_Report_Lapse_Register_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 14/03/2001
** RSA Reports - Lapse_Register.rpt
**
**********************************************************************************************************************************
** 04/05/2001   Jude Killip     stop future lapse dates included
**
** 06/06/2001   Jude Killip     don't need class of business
**
** 25/06/2001   Jude Killip     Allow future lapse dates...
**
** 19/09/2001   Jude Killip     Add date parameters
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Lapse_Register_SFU 
                (@register_period varchar (20),
                @start_date datetime,
                @end_date datetime
                )
AS

/*
--for testing
declare @start_date datetime,
    @end_date datetime,
    @register_period varchar (20)

select @start_date = dateadd(day,-55,getdate()),
    @end_date = getdate(),
    @register_period = 'Yesterday'
*/

SELECT  ifo.insurance_folder_cnt,
        ifi.insurance_ref,
        py.resolved_name,
        ifi.cover_start_date,
        ifi.expiry_date,
        ifi.lapsed_date,
        ifi.lapsed_description,
        pr.product_id,
        pr.description 'product',
        rt.risk_type_id,
        rt.description 'risk type'

FROM insurance_file ifi
JOIN product pr                         ON ifi.product_id = pr.product_id
JOIN insurance_folder ifo               ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
JOIN party py                           ON py.party_cnt = ifo.insurance_holder_cnt
JOIN insurance_file_risk_link ifrl      ON ifi.insurance_file_cnt = ifrl.insurance_file_cnt
JOIN risk r                             ON ifrl.risk_cnt = r.risk_cnt
JOIN risk_type rt                       ON r.risk_type_id = rt.risk_type_id
WHERE ifi.insurance_file_status_id = 2                  -- 2=Lapsed
AND (
    @register_period = 'specify dates' AND 
        (
        datediff(day, @start_date, ifi.lapsed_date) >=0
        AND datediff(day, ifi.lapsed_date, @end_date) >=0
        )
    OR
    @register_period = 'yesterday' AND
    datediff (day, ifi.lapsed_date, getdate())= 1
    OR
    @register_period = 'today' AND
    datediff (day, ifi.lapsed_date, getdate())= 0
    OR
    @register_period = 'last full week' AND
    datediff (week, ifi.lapsed_date, getdate())= 1
    OR 
    @register_period = 'this week' AND
    datediff (week, ifi.lapsed_date, getdate())= 0    
    OR
    @register_period = 'last full month' AND
    datediff (month, ifi.lapsed_date, getdate())= 1
    OR
    @register_period = 'this month' AND
    datediff (month, ifi.lapsed_date, getdate())= 0    
    )

GO