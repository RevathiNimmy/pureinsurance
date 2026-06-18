SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Party'
GO
CREATE PROCEDURE spu_SAM_Get_Party
    @Party_Cnt int

AS
BEGIN

        SELECT ae.resolved_name as AccountExec,
               c.description as PartyCurrency,
               cc.no_of_offices as NoOfOffices,
               eb.EmployeeBand_id as NoOfEmployees,
               ag.resolved_name as Agent,
               -- Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
               ae.shortname as AccountExecCode
               -- End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
          FROM party p  
     LEFT JOIN party ae ON p.consultant_cnt = ae.party_cnt  
     LEFT JOIN party ag ON p.agent_cnt = ag.party_cnt  
     LEFT JOIN currency c ON p.currency_id = c.currency_id  
     LEFT JOIN party_corporate_client cc ON p.party_cnt = cc.party_cnt  
     LEFT JOIN EmployeeBand eb ON cc.no_of_employees = eb.employeeband_id  
         WHERE p.party_cnt = @Party_Cnt  
END  
GO
