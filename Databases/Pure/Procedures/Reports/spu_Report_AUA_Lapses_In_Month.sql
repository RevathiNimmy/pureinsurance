SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_AUA_Lapses_In_Month'
GO

CREATE PROCEDURE spu_Report_AUA_Lapses_In_Month
                @start_date datetime,
                @end_date datetime
AS
SET NOCOUNT ON
CREATE TABLE #tmpAUALapsesInMonth
(
    AgentName           varchar(255) NULL,
    ClassOfBusiness         varchar(255) NULL,
    PolicyNo            varchar(30) NULL,
        Insured             varchar(30) NULL,
    LapseDate           datetime  NULL,
    GrossPremium            decimal(19,4) NULL,
    Reason_for_lapse        varchar(255) NULL
)
INSERT INTO #tmpAUALapsesInMonth
    select  
        p.description,
        pa.trading_name,
        insurance_ref, resolved_name,
        lapsed_date, this_premium,lapsed_description
    from insurance_file ifi
    join product  p on ifi.product_id = p.product_id
    join party_agent pa on lead_agent_cnt = pa.party_cnt
    join insurance_folder ifo on ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
    join party py on  ifo.insurance_holder_cnt = py.party_cnt
    where lapsed_date IS NOT NULL and (lapsed_date BETWEEN @Start_date and @End_Date)
SET NOCOUNT OFF
Select * FROM #tmpauaLapsesInMonth
DROP TABLE #tmpauaLapsesInMonth
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

