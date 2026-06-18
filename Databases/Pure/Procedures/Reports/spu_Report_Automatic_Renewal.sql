SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Report_Automatic_Renewal'
GO


CREATE PROCEDURE spu_Report_Automatic_Renewal
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 07/09/2000 
** RSA Reports - AutomaticRenewal.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 11/09/2000   Jude Killip     removed all dummy data
**
** 17/11/2000   Jude Killip     change from "AutomaticRenewal" to "AutoRenewal"
**                              ...and remove aliases
** 04/06/2001   RWH        Retrieve username to enable filtering within the report.
**
** 19/07/2001   JMK         retrieve client shortname to make sorting more sensible
**
** 25/07/2001   JMK         make sure subquery only returns one record
** 20/03/2002   Kerry       Pick up branch detail
** 15/03/2005	JT	    Pick up Account_handler
***********************************************************************************************************************************/
SELECT rr.client_name,
        rr.policy_number,
        rr.agent_code,
        rr.cover_start_date,
        rr.cover_end_date,
        rr.product_code,
        u.username,
        (SELECT max(shortname) FROM party WHERE resolved_name = client_name) client_code,
        (SELECT source.code FROM source WHERE
            source_id = (SELECT max(source_id) FROM Insurance_File WHERE insurance_file.insurance_ref = rr.policy_number))branch_code,
	(SELECT max([name]) FROM party, Insurance_File WHERE insurance_file.insurance_ref = rr.policy_number	
	 and party_cnt = Insurance_File.account_handler_cnt ) Account_handler

	
            
FROM    Renewal_Report rr,
        PMUser u
WHERE rr.report_type = 'AutoRenewal'
AND u.user_id = rr.user_id
GO


