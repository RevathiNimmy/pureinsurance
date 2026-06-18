SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Renewal_Status'
GO


CREATE PROCEDURE spu_Report_Renewal_Status
    @product varchar(255),
    @agent varchar(255),
    @start_date datetime,
    @end_date datetime,
    @branch_id int,
    @RenewalStatus varchar(255)
AS

/**********************************************************************************************************************************
** Created by Jude Killip
**
** USED BY:     U/W Reports - Renewal_Status.rpt
**
**********************************************************************************************************************************
** 12/02/2001   Jude Killip     Created
**
** 16/06/2001   Jude Killip     Longer names and descriptions instead of codes
**                              Dates filter in SP
**
** 06/08/2001   Jude Killip     Use cover_start_date as selection filter, not renewal_date
**
** 07/08/2001   Jude Killip     Amend agent join to include direct business
**                              Use cover start date (of renewal version) for RenewalDate
**                              Make date parameters inclusive
**
** 08/08/2001   Jude Killip     Default Agent name to ' Direct' to indicate direct business
**                              ...and make sure it appears on the report
**
** 06/09/2001   Jude Killip     get renewal status description instead of code
***********************************************************************************************************************************/
Set Nocount ON
CREATE TABLE #tempRSARen_Status
(
    Client varchar(100) NULL,
    PolicyNumber varchar(30) NULL,
    Agent varchar (100) NULL,
    Product varchar (255) NULL,
    RenewalDate datetime NULL,
    Premium money NULL,
    RenewalStatus varchar(255) NULL
)

INSERT INTO #tempRSARen_Status
        SELECT pHolder.resolved_name,
                ifi.insurance_ref,
                isnull(pAgent.resolved_name,' Direct'),
                p.description,
                ifi.cover_start_date,
                ifi.this_premium,
                rst.description
        FROM renewal_status rs
        JOIN insurance_file ifi ON ifi.insurance_file_cnt = rs.renewal_insurance_file_cnt               -- Renewal Policy
        JOIN renewal_status_type rst ON rst.renewal_status_type_id = rs.renewal_status_type_id
        JOIN product p ON rs.product_id = p.product_id
        JOIN party pHolder ON rs.insurance_holder_cnt = pHolder.party_cnt
        LEFT OUTER JOIN party pAgent ON rs.lead_agent_cnt = pAgent.party_cnt
        WHERE ifi.cover_start_date >= @start_date
        AND ifi.cover_start_date <= @end_date
        AND (ifi.source_id = @branch_id OR ISNULL(@branch_id, 0) = 0)
        AND (rst.description = @RenewalStatus OR isnull(@RenewalStatus, 'ALL') = 'ALL')

Set Nocount OFF
IF isnull(@product, 'ALL') = 'ALL' AND isnull(@agent, 'ALL') = 'ALL'
        SELECT * from #tempRSARen_Status
ELSE IF isnull(@product, 'ALL') <> 'ALL' AND isnull(@agent, 'ALL') <> 'ALL'
BEGIN
        SELECT * from #tempRSARen_Status
        WHERE Product = @product
        AND Agent = @agent
END
ELSE IF isnull(@product, 'ALL') <> 'ALL'
BEGIN
        SELECT * from #tempRSARen_Status
        WHERE Product = @product
END
ELSE
BEGIN
        SELECT * from #tempRSARen_Status
        WHERE Agent = @agent
END

DROP TABLE #tempRSARen_Status
GO

