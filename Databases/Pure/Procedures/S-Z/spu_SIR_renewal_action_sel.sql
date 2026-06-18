SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_action_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_action_sel
AS

/*
    CTAF: Reason why this might fail : The PMCaption tables are out of sync.
*/
BEGIN
    SELECT rtrim(rst.code) as status,
              ra.pmwrk_task_id,
              ra.code,
              rtrim(c.caption),
          ra.is_hotkey,
          ra.renewal_type
    FROM Renewal_Action ra
    INNER JOIN PMCaption c ON ra.caption_id = c.caption_id
    INNER JOIN Renewal_Status_Type rst ON ra.renewal_status_type_id = rst.renewal_status_type_id
    /* AK 011001 - just to keep the actions priority in the menu */
    /* ORDER BY rst.code, c.caption */

END
GO


