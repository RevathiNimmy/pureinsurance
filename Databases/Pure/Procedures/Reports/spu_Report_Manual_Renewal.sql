SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Report_Manual_Renewal'
GO

CREATE PROCEDURE spu_Report_Manual_Renewal  
AS  
  
/**********************************************************************************************************************************  
** Created by Jude Killip  
** 07/09/2000  
** RSA Reports - ManualRenewal.rpt  
**  
**********************************************************************************************************************************  
** JMK  09/09/2000          amended to use Renewal_Report  
** JMK  15/11/2000          Renewal_Report change  
** RWH  04/06/2001          Retrieve username now associated with each record. Report will be filtered on this inside  
**                          report itself at present.  
**  
** 19/07/2001   JMK         retrieve client shortname to make sorting more sensible  
**  
** 25/07/2001   JMK         make sure subquery only returns one record  
** 20/03/2002   Kerry       Pick up the branch code as well  
** 15/03/2005 	JT     	    Pick up Account_handler  
** 12-04-2005 	MEvans      Added Number of Claims that exists for the policy number
** 04-08-2008 	SURSINGH    Added Claims version filter to select Number of claims that exists for the policy number
***********************************************************************************************************************************/  
/*  
        SELECT client_name,  
                policy_number,  
                agent_code,  
                cover_start_date,  
                cover_end_date,  
                product_code,  
                failure_criterion,  
                failure_detail  
        FROM Renewal_Report  
        WHERE report_type = 'ManualRenewal'  
*/  
    SELECT rr.client_name,  
        rr.policy_number,  
        rr.agent_code,  
        rr.cover_start_date,  
        rr.cover_end_date,  
        rr.product_code,  
        rr.failure_criterion,  
        rr.failure_detail,  
        u.username,  
        (select max(shortname) from party where resolved_name = client_name) client_code,  
        (SELECT source.code FROM source WHERE  
            source_id = (SELECT max(source_id) FROM Insurance_File WHERE insurance_file.insurance_ref = rr.policy_number))branch_code,  
        (SELECT max(shortname) FROM party, Insurance_File WHERE insurance_file.insurance_ref = rr.policy_number   
   and party_cnt = Insurance_File.account_handler_cnt ) Account_handler , 
	ISNULL(Claim.NoOfClaims, 0) as NoOFClaims
  
    FROM    Renewal_Report rr

	LEFT JOIN PMUser u ON 
		u.user_id = rr.user_id  

    LEFT JOIN (select policy_number, count(*) as noofclaims from claim Where Version_id=1 
  	       group by policy_number) Claim ON rr.policy_number = Claim.Policy_Number

    WHERE rr.report_type = 'ManualRenewal'  
	
 






GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
