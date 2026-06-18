SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_accumulation_by_level_saa'
GO


-- RAW 04/09/2003 : CQ258 : add @v_iIncludeDeleted param and test is_deleted columns


CREATE PROCEDURE spu_accumulation_by_level_saa
    @accumulation_level int,
    @language_id int,
    @effective_date datetime, 
    @v_iIncludeDeleted int = 1
AS


IF @accumulation_level = 1
    SELECT  a1.accumulation_id,
        pc.caption,
        a1.code,
        a1.is_deleted
    FROM    PMCaption pc,
        accumulation a1
    WHERE   pc.caption_id = a1.caption_id
    AND pc.language_id = @language_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    UNION
    SELECT  a2.accumulation_id,
        pc.caption,
        a2.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            ELSE a2.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2
    WHERE   pc.caption_id = a2.caption_id
    AND pc.language_id = @language_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    UNION
    SELECT  a3.accumulation_id,
        pc.caption,
        a3.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            ELSE a3.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3
    WHERE   pc.caption_id = a3.caption_id
    AND pc.language_id = @language_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    UNION
    SELECT  a4.accumulation_id,
        pc.caption,
        a4.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            ELSE a4.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4
    WHERE   pc.caption_id = a4.caption_id
    AND pc.language_id = @language_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    UNION
    SELECT  a5.accumulation_id,
        pc.caption,
        a5.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            ELSE a5.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5
    WHERE   pc.caption_id = a5.caption_id
    AND pc.language_id = @language_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    UNION
    SELECT  a6.accumulation_id,
        pc.caption,
        a6.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            ELSE a6.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,        accumulation a4,
        accumulation a5,
        accumulation a6
    WHERE   pc.caption_id = a6.caption_id
    AND pc.language_id = @language_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    UNION
    SELECT  a7.accumulation_id,
        pc.caption,
        a7.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            ELSE a7.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7
    WHERE   pc.caption_id = a7.caption_id
    AND pc.language_id = @language_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id   AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    UNION
    SELECT  a8.accumulation_id,
        pc.caption,
        a8.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            ELSE a8.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8
    WHERE   pc.caption_id = a8.caption_id
    AND pc.language_id = @language_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    UNION
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
ELSE
IF @accumulation_level = 2
    SELECT  a2.accumulation_id,
        pc.caption,
        a2.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            ELSE a2.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2
    WHERE   pc.caption_id = a2.caption_id
    AND pc.language_id = @language_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    UNION
    SELECT  a3.accumulation_id,
        pc.caption,
        a3.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            ELSE a3.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3
    WHERE   pc.caption_id = a3.caption_id
    AND pc.language_id = @language_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    UNION
    SELECT  a4.accumulation_id,
        pc.caption,
        a4.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            ELSE a4.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4
    WHERE   pc.caption_id = a4.caption_id
    AND pc.language_id = @language_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    UNION
    SELECT  a5.accumulation_id,
        pc.caption,
        a5.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            ELSE a5.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5
    WHERE   pc.caption_id = a5.caption_id
    AND pc.language_id = @language_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    UNION
    SELECT  a6.accumulation_id,
        pc.caption,
        a6.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            ELSE a6.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,        accumulation a4,
        accumulation a5,
        accumulation a6
    WHERE   pc.caption_id = a6.caption_id
    AND pc.language_id = @language_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    UNION
    SELECT  a7.accumulation_id,
        pc.caption,
        a7.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            ELSE a7.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7
    WHERE   pc.caption_id = a7.caption_id
    AND pc.language_id = @language_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    UNION
    SELECT  a8.accumulation_id,
        pc.caption,
        a8.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            ELSE a8.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8
    WHERE   pc.caption_id = a8.caption_id
    AND pc.language_id = @language_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    UNION
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
ELSE
IF @accumulation_level = 3
    SELECT  a3.accumulation_id,
        pc.caption,
        a3.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            ELSE a3.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3
    WHERE   pc.caption_id = a3.caption_id
    AND pc.language_id = @language_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    UNION
    SELECT  a4.accumulation_id,
        pc.caption,
        a4.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            ELSE a4.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4
    WHERE   pc.caption_id = a4.caption_id
    AND pc.language_id = @language_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    UNION
    SELECT  a5.accumulation_id,
        pc.caption,
        a5.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            ELSE a5.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5
    WHERE   pc.caption_id = a5.caption_id
    AND pc.language_id = @language_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    UNION
    SELECT  a6.accumulation_id,
        pc.caption,
        a6.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            ELSE a6.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,        accumulation a4,
        accumulation a5,
        accumulation a6
    WHERE   pc.caption_id = a6.caption_id
    AND pc.language_id = @language_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    UNION
    SELECT  a7.accumulation_id,
        pc.caption,
        a7.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            ELSE a7.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7
    WHERE   pc.caption_id = a7.caption_id
    AND pc.language_id = @language_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    UNION
    SELECT  a8.accumulation_id,
        pc.caption,
        a8.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            ELSE a8.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8
    WHERE   pc.caption_id = a8.caption_id
    AND pc.language_id = @language_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    UNION
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
ELSE
IF @accumulation_level = 4
    SELECT  a4.accumulation_id,
        pc.caption,
        a4.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            ELSE a4.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4
    WHERE   pc.caption_id = a4.caption_id
    AND pc.language_id = @language_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    UNION
    SELECT  a5.accumulation_id,
        pc.caption,
        a5.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            ELSE a5.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5
    WHERE   pc.caption_id = a5.caption_id
    AND pc.language_id = @language_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    UNION
    SELECT  a6.accumulation_id,
        pc.caption,
        a6.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            ELSE a6.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,        accumulation a4,
        accumulation a5,
        accumulation a6
    WHERE   pc.caption_id = a6.caption_id
    AND pc.language_id = @language_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    UNION
    SELECT  a7.accumulation_id,
        pc.caption,
        a7.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            ELSE a7.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7
    WHERE   pc.caption_id = a7.caption_id
    AND pc.language_id = @language_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    UNION
    SELECT  a8.accumulation_id,
        pc.caption,
        a8.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            ELSE a8.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8
    WHERE   pc.caption_id = a8.caption_id
    AND pc.language_id = @language_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    UNION
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
ELSE
IF @accumulation_level = 5
    SELECT  a5.accumulation_id,
        pc.caption,
        a5.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            ELSE a5.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5
    WHERE   pc.caption_id = a5.caption_id
    AND pc.language_id = @language_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    UNION
    SELECT  a6.accumulation_id,
        pc.caption,
        a6.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            ELSE a6.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,        accumulation a4,
        accumulation a5,
        accumulation a6
    WHERE   pc.caption_id = a6.caption_id
    AND pc.language_id = @language_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    UNION
    SELECT  a7.accumulation_id,
        pc.caption,
        a7.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            ELSE a7.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7
    WHERE   pc.caption_id = a7.caption_id
    AND pc.language_id = @language_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    UNION
    SELECT  a8.accumulation_id,
        pc.caption,
        a8.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            ELSE a8.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8
    WHERE   pc.caption_id = a8.caption_id
    AND pc.language_id = @language_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    UNION
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
ELSE
IF @accumulation_level = 6
    SELECT  a6.accumulation_id,
        pc.caption,
        a6.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            ELSE a6.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,        accumulation a4,
        accumulation a5,
        accumulation a6
    WHERE   pc.caption_id = a6.caption_id
    AND pc.language_id = @language_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    UNION
    SELECT  a7.accumulation_id,
        pc.caption,
        a7.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            ELSE a7.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7
    WHERE   pc.caption_id = a7.caption_id
    AND pc.language_id = @language_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    UNION
    SELECT  a8.accumulation_id,
        pc.caption,
        a8.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            ELSE a8.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8
    WHERE   pc.caption_id = a8.caption_id
    AND pc.language_id = @language_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    UNION
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
ELSE
IF @accumulation_level = 7
    SELECT  a7.accumulation_id,
        pc.caption,
        a7.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            ELSE a7.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7
    WHERE   pc.caption_id = a7.caption_id
    AND pc.language_id = @language_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    UNION
    SELECT  a8.accumulation_id,
        pc.caption,
        a8.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            ELSE a8.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8
    WHERE   pc.caption_id = a8.caption_id
    AND pc.language_id = @language_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    UNION
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
ELSE
IF @accumulation_level = 8
    SELECT  a8.accumulation_id,
        pc.caption,
        a8.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            ELSE a8.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8
    WHERE   pc.caption_id = a8.caption_id
    AND pc.language_id = @language_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    UNION
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
ELSE
IF @accumulation_level = 9
    SELECT  a9.accumulation_id,
        pc.caption,
        a9.code,
        CASE
            WHEN a1.is_deleted = 1 THEN 1
            WHEN a2.is_deleted = 1 THEN 1
            WHEN a3.is_deleted = 1 THEN 1
            WHEN a4.is_deleted = 1 THEN 1
            WHEN a5.is_deleted = 1 THEN 1
            WHEN a6.is_deleted = 1 THEN 1
            WHEN a7.is_deleted = 1 THEN 1
            WHEN a8.is_deleted = 1 THEN 1
            ELSE a9.is_deleted
        END
    FROM    PMCaption pc,
        accumulation a1,
        accumulation a2,
        accumulation a3,
        accumulation a4,
        accumulation a5,
        accumulation a6,
        accumulation a7,
        accumulation a8,
        accumulation a9
    WHERE   pc.caption_id = a9.caption_id
    AND pc.language_id = @language_id
    AND a9.parent_id = a8.accumulation_id
    AND a8.parent_id = a7.accumulation_id
    AND a7.parent_id = a6.accumulation_id
    AND a6.parent_id = a5.accumulation_id
    AND a5.parent_id = a4.accumulation_id
    AND a4.parent_id = a3.accumulation_id
    AND a3.parent_id = a2.accumulation_id
    AND a2.parent_id = a1.accumulation_id
    AND a1.parent_id IS NULL
    AND a1.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a1.is_deleted ELSE 0 END
    AND a2.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a2.is_deleted ELSE 0 END
    AND a3.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a3.is_deleted ELSE 0 END
    AND a4.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a4.is_deleted ELSE 0 END
    AND a5.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a5.is_deleted ELSE 0 END
    AND a6.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a6.is_deleted ELSE 0 END
    AND a7.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a7.is_deleted ELSE 0 END
    AND a8.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a8.is_deleted ELSE 0 END
    AND a9.is_deleted = CASE WHEN @v_iIncludeDeleted = 1 THEN a9.is_deleted ELSE 0 END
GO



