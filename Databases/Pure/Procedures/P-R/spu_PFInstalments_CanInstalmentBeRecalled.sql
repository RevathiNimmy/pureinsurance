EXECUTE DDLDropProcedure 'spu_PFInstalments_CanInstalmentBeRecalled'
GO
Execute DDLDropProcedure 'spu_PFInstalments_CanInstalmentBeRecalled'
GO

CREATE PROCEDURE spu_PFInstalments_CanInstalmentBeRecalled
	@user_id SMALLINT,
	@pfinstalments_id INT,
	@can_be_recalled TINYINT OUTPUT,
        @allocation_days_exceeded TINYINT OUTPUT
AS

DECLARE @allow_reverse_allocations TINYINT
DECLARE @reverse_allocations_days SMALLINT
DECLARE @paid_date DATEtime

SELECT	@allow_reverse_allocations=ISNULL(allow_reverse_allocations,0),
	@reverse_allocations_days=ISNULL(reverse_allocations_days,0)
FROM	User_Authorities
WHERE	user_id=@user_id

IF @allow_reverse_allocations=0
	SET @can_be_recalled=0
ELSE BEGIN
	SELECT @paid_date=posteddate FROM	PFInstalments WHERE	pfinstalments_id=@pfinstalments_id AND	Status=3
	SET @can_be_recalled=@allow_reverse_allocations
    SET @allocation_days_exceeded = 0

	IF @paid_date IS NULL
		SET @can_be_recalled=0
	ELSE IF ABS(DATEDIFF(day,@paid_date, GETDATE())) > @reverse_allocations_days
	BEGIN
		--SET @can_be_recalled=0
		SET @allocation_days_exceeded = 1
    END

END
Go

