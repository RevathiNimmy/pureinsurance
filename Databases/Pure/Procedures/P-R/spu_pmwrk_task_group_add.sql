SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_group_add'
GO


CREATE PROCEDURE spu_pmwrk_task_group_add
    @caption_id INT,
    @code VARCHAR(10),
    @description VARCHAR(255),
    @is_deleted SMALLINT,
    @effective_date DATETIME,
    @display_icon INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 20/08/1997 JW */
/* 1.1 Link to task group category 07/10/1999 DAK */
/* 1.2 Remove Task Group Category 21/12/1999 DAK */
/********************************************************************************************************/
DECLARE @pmwrk_task_group_id INT

insert into PMWrk_Task_Group(
                caption_id,
                code,
                description,
                is_deleted,
                effective_date,
                display_icon)
values (
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date,
    @display_icon)

SET @pmwrk_task_group_id=@@IDENTITY
SELECT @pmwrk_task_group_id AS pmwrk_task_group_id

GO


