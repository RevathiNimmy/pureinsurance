SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_create_report_risk_and_group_tables'
GO
CREATE PROCEDURE spu_create_report_risk_and_group_tables
AS
BEGIN
    EXECUTE DDLDropTable 'IncludedRisks'

    EXECUTE DDLDropTable 'ExcludedRisks'

    EXECUTE DDLDropTable 'IncludedRiskGroups'

    EXECUTE DDLDropTable 'ExcludedRiskGroups'

    EXECUTE DDLDropTable 'Report_Groups'

    CREATE TABLE IncludedRisks (
        riskcodeid integer
    )

    CREATE TABLE ExcludedRisks (
        riskcodeid integer
    )

    CREATE TABLE IncludedRiskGroups (
        riskgroupid integer
    )

    CREATE TABLE ExcludedRiskGroups (
        riskgroupid integer
    )

    CREATE TABLE Report_Groups (
        [group] varchar(50)
    )
END
GO

