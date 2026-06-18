
EXECUTE DDLDropProcedure 'spu_Report_Party_Listing_Accident_SFU'
GO

--*******************************************************************************************
-- Created by Jude Killip
-- 31/10/2001
-- RSA Reports - Party_Listing.rpt: Accidents subreport
--
--*******************************************************************************************

--*******************************************************************************************
CREATE PROCEDURE spu_Report_Party_Listing_AccidenT_SFU

AS

SELECT party_cnt,
    [Date] accident_date,
    Description,
    CASE is_at_fault
        WHEN 1 THEN 'yes' ELSE 'no'
    END
FROM previous_accidents


GO