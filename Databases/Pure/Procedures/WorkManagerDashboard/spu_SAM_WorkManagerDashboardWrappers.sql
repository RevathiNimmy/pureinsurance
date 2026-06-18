SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

-- =============================================
-- Wrapper SPs called by the REST API via CallNamedStoredProcedure
-- These translate API parameter conventions to inner SP parameters
-- =============================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerDashboardSummary') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerDashboardSummary
GO

CREATE PROCEDURE spu_SAM_WorkManagerDashboardSummary
    @suser_group_ids VARCHAR(MAX) = NULL,
    @sbranch_ids VARCHAR(MAX) = NULL,
    @ldate_range INT = 1
AS
BEGIN
    IF @suser_group_ids = '' SET @suser_group_ids = NULL
    IF @sbranch_ids = '' SET @sbranch_ids = NULL
    EXEC spu_SAM_WorkManagerSummary @user_group_ids = @suser_group_ids, @branch_ids = @sbranch_ids, @date_range = @ldate_range
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerDashboardTrend') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerDashboardTrend
GO

CREATE PROCEDURE spu_SAM_WorkManagerDashboardTrend
    @suser_group_ids VARCHAR(MAX) = NULL,
    @sbranch_ids VARCHAR(MAX) = NULL
AS
BEGIN
    IF @suser_group_ids = '' SET @suser_group_ids = NULL
    IF @sbranch_ids = '' SET @sbranch_ids = NULL
    EXEC spu_SAM_WorkManagerTrend @user_group_ids = @suser_group_ids, @branch_ids = @sbranch_ids
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerDashboardTasksByGroup') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerDashboardTasksByGroup
GO

CREATE PROCEDURE spu_SAM_WorkManagerDashboardTasksByGroup
    @suser_group_ids VARCHAR(MAX) = NULL,
    @sbranch_ids VARCHAR(MAX) = NULL
AS
BEGIN
    IF @suser_group_ids = '' SET @suser_group_ids = NULL
    IF @sbranch_ids = '' SET @sbranch_ids = NULL
    EXEC spu_SAM_WorkManagerTasksByGroup @user_group_ids = @suser_group_ids, @branch_ids = @sbranch_ids
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerDashboardCompletedVsTotal') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerDashboardCompletedVsTotal
GO

CREATE PROCEDURE spu_SAM_WorkManagerDashboardCompletedVsTotal
    @suser_group_ids VARCHAR(MAX) = NULL,
    @sbranch_ids VARCHAR(MAX) = NULL,
    @lmonths INT = 6
AS
BEGIN
    IF @suser_group_ids = '' SET @suser_group_ids = NULL
    IF @sbranch_ids = '' SET @sbranch_ids = NULL
    EXEC spu_SAM_WorkManagerCompletedVsTotal @user_group_ids = @suser_group_ids, @branch_ids = @sbranch_ids, @months = @lmonths
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerDashboardTasksByUser') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerDashboardTasksByUser
GO

CREATE PROCEDURE spu_SAM_WorkManagerDashboardTasksByUser
    @suser_group_ids VARCHAR(MAX) = NULL,
    @sbranch_ids VARCHAR(MAX) = NULL
AS
BEGIN
    IF @suser_group_ids = '' SET @suser_group_ids = NULL
    IF @sbranch_ids = '' SET @sbranch_ids = NULL
    EXEC spu_SAM_WorkManagerTasksByUser @user_group_ids = @suser_group_ids, @branch_ids = @sbranch_ids
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerDashboardTasksDue') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerDashboardTasksDue
GO

CREATE PROCEDURE spu_SAM_WorkManagerDashboardTasksDue
    @suser_group_ids VARCHAR(MAX) = NULL,
    @sbranch_ids VARCHAR(MAX) = NULL,
    @lfor_user_id INT = NULL,
    @ldate_range INT = 1
AS
BEGIN
    IF @suser_group_ids = '' SET @suser_group_ids = NULL
    IF @sbranch_ids = '' SET @sbranch_ids = NULL
    IF @lfor_user_id = 0 SET @lfor_user_id = NULL
    EXEC spu_SAM_WorkManagerTasksDue @user_group_ids = @suser_group_ids, @branch_ids = @sbranch_ids, @for_user_id = @lfor_user_id, @date_range = @ldate_range
END
GO
