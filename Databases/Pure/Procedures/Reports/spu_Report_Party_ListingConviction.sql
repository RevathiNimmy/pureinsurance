SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Party_ListingConviction'
GO


CREATE PROCEDURE spu_Report_Party_ListingConviction
AS
--*******************************************************************************************
-- Created by Jude Killip
-- 31/10/2001
-- RSA Reports - Party_Listing.rpt: Convictions subreport
--
--*******************************************************************************************

--*******************************************************************************************
SELECT party_cnt,
        code,
        conviction_date,
        description,
        fine_amt,
        sentence_code,
        sentence_description,
        sentence_duration,
        sentence_duration_qualifier,
        sentence_effective_date,
        status_code,
        alcohol_level,
        alcohol_measurement_method,
        driving_licence_penalty_pts
FROM party_conviction


GO