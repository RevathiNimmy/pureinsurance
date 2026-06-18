IF EXISTS
    (
        SELECT control_caption 
		FROM System_Option_Configuration
		WHERE control_caption LIKE 'Enable Exclusive Locking:' AND option_number=5115
    )
BEGIN    
	UPDATE System_Option_Configuration SET option_number=5174 WHERE option_number=5115
END
GO

IF EXISTS
    (
        SELECT option_number 
		FROM System_Options
		WHERE description LIKE 'Enable Exclusive Locking:' AND option_number=5115
    )
BEGIN
    DELETE System_Options WHERE option_number = 5174
	UPDATE System_Options SET option_number=5174 WHERE option_number=5115
END
GO


DECLARE CUR CURSOR FOR 
SELECT COUNT(OPTION_NUMBER),DESCRIPTION FROM SYSTEM_OPTIONS WHERE DESCRIPTION IS NOT NULL AND DESCRIPTION <> '' AND BRANCH_ID=1 
					AND description NOT IN ('Apply Back-Dated Risk Editing Restrictions','Document Opening Delay:')
					 GROUP BY DESCRIPTION HAVING COUNT(OPTION_NUMBER)>1

OPEN CUR
DECLARE @OPT_CNT INT
DECLARE @OPT_NUM INT
DECLARE @OPT_NUM_TOCHECK INT
DECLARE @OPT_NUM_TOUPD INT
DECLARE @CNT_CAP VARCHAR(255)
DECLARE @DESC_TOUPD VARCHAR(255)
FETCH NEXT FROM CUR INTO @OPT_CNT,@CNT_CAP
WHILE (@@FETCH_STATUS = 0)
BEGIN
	SET @OPT_NUM_TOUPD = NULL
--The new option number on upgrade from system_option_configuration table
	SELECT @OPT_NUM = OPTION_NUMBER FROM SYSTEM_OPTION_CONFIGURATION WHERE CONTROL_CAPTION=@CNT_CAP
	
--The old option number in system_options table
	SET @OPT_NUM_TOCHECK = (SELECT TOP 1 OPTION_NUMBER FROM SYSTEM_OPTIONS WHERE DESCRIPTION=@CNT_CAP AND OPTION_NUMBER <> @OPT_NUM ORDER BY OPTION_NUMBER DESC)
	
--Update the values from old option_number to new option_number
	UPDATE SYSTEM_OPTIONS SET VALUE = (SELECT VALUE FROM SYSTEM_OPTIONS WHERE OPTION_NUMBER = @OPT_NUM_TOCHECK AND BRANCH_ID=1) WHERE OPTION_NUMBER=@OPT_NUM

--Delete the old option_number which has now been replaced with the new one.
	DELETE FROM SYSTEM_OPTIONS WHERE OPTION_NUMBER IN (SELECT OPTION_NUMBER FROM SYSTEM_OPTIONS WHERE DESCRIPTION=@CNT_CAP AND OPTION_NUMBER <> @OPT_NUM)

--Now you will have to update the already existing option_number aswell insert the missing option_number in system_option table which is done below.
	SELECT @DESC_TOUPD = CONTROL_CAPTION FROM SYSTEM_OPTION_CONFIGURATION WHERE OPTION_NUMBER=@OPT_NUM_TOCHECK AND CONTROL_CAPTION IS NOT NULL
	
	IF EXISTS (SELECT 1 FROM SYSTEM_OPTIONS WHERE DESCRIPTION = @DESC_TOUPD)
	BEGIN
		SELECT @OPT_NUM_TOUPD= OPTION_NUMBER FROM System_Options WHERE DESCRIPTION = @DESC_TOUPD
	
		UPDATE SYSTEM_OPTIONS SET OPTION_NUMBER=@OPT_NUM_TOCHECK WHERE DESCRIPTION = @DESC_TOUPD
	END

	INSERT INTO SYSTEM_OPTIONS (BRANCH_ID, OPTION_NUMBER, VALUE, DESCRIPTION)
	SELECT DISTINCT S.SOURCE_ID, SOC.OPTION_NUMBER, NULL, MAX(SOC.CONTROL_CAPTION)
		FROM SYSTEM_OPTION_CONFIGURATION SOC, SOURCE S
		WHERE SOC.OPTION_NUMBER = ISNULL(@OPT_NUM_TOUPD,@OPT_NUM_TOCHECK)
		GROUP BY S.SOURCE_ID, SOC.OPTION_NUMBER
		ORDER BY SOURCE_ID, OPTION_NUMBER

    FETCH NEXT FROM CUR INTO @OPT_CNT,@CNT_CAP
END
CLOSE CUR
DEALLOCATE CUR
-- *****************************************************************************
-- * Author:   Vijay Pal
-- * Date:     20/11/2013
-- * Purpose:  Password Control
-- *****************************************************************************
declare @val varchar(20)
select @val = value  from System_Options where option_number= 5101
--select @descr
if @val is NULL
begin
UPDATE system_options SET value ='^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*[\W_\x7B-\xFF]).{8,15}$' where option_number = 5101
UPDATE system_options SET value ='30' where option_number = 5103
UPDATE system_options SET value ='3' where option_number = 5105
UPDATE system_options SET value ='3' where option_number = 5107
UPDATE system_options SET value ='1' where option_number = 5109
UPDATE system_options SET value ='10' where option_number = 5111
UPDATE system_options SET value ='Password must be between eight and fifteen characters in length, be a mix of upper and lowercase letters, and contain at least one number. Special characters are not permitted' where option_number = 5113
End

Go

UPDATE SYSTEM_OPTIONS SET VALUE=0 WHERE OPTION_NUMBER IN (5098,5260,5259,5258) AND VALUE IS NULL

Go