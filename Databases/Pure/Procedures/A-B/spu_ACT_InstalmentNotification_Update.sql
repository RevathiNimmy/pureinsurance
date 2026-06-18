
--Start(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(6.4.1.2)

SET QUOTED_IDENTIFIER OFF
EXECUTE DDLDropProcedure 'spu_ACT_InstalmentNotification_Update'
GO

CREATE PROCEDURE spu_ACT_InstalmentNotification_Update
	@pfinstalments_id int,
	@notification_sent tinyint
AS
UPDATE
	PFInstalments 
SET
	notification_sent = @notification_sent
WHERE 
	pfinstalments_id = @pfinstalments_id
--End(Saurabh Agrawal) Tech Spec PGR005 Automated Emails(6.4.1.2)