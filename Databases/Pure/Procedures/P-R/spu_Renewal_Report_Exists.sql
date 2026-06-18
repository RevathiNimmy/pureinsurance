SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Renewal_Report_Exists'
GO


CREATE PROCEDURE spu_Renewal_Report_Exists
    @report_type varchar(30),
    @user_id int
AS

/**********************************************************************************************************************************
** Created by RWH
** 08/06/2001
**
**To check for existence of renewal report records of a specific type for a given user.
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
SELECT client_name,
        policy_number,
        agent_code,
        cover_start_date,
        cover_end_date,
        product_code,
        failure_criterion,
        failure_detail
    FROM    Renewal_Report

    WHERE report_type = @report_type
    AND  user_id = @user_id
GO


