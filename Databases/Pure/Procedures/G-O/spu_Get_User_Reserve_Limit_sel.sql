SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_User_Reserve_Limit_sel'
GO

CREATE PROCEDURE spu_Get_User_Reserve_Limit_sel
(
    @UserName VARCHAR(255),
	@TransactionType VARCHAR(10),
	@PerilID INT = NULL,
	@ReserveLimit MONEY OUTPUT
)

AS

IF @TransactionType = 'C_CO'
BEGIN
	SELECT @ReserveLimit = URL.[Reserve_Aggregate_Limit]
		FROM claim_reserve_limit URL
	WHERE URL.User_Name = @UserName
END

ELSE IF @TransactionType = 'C_CR'
BEGIN
	SELECT @ReserveLimit = URL.[Reserve_Aggregate_Limit]
		FROM claim_reserve_limit URL
	WHERE URL.User_Name = @UserName
END

GO