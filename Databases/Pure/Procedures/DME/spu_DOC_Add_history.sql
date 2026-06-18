SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_DOC_Add_history'
GO
CREATE PROCEDURE spu_DOC_Add_history
    @history_id integer OUTPUT,
    @task smallint,
    @cabinetcode char(20),
    @cabinetname char(30),
    @drawercode char(20),
    @drawername char(30),
    @foldercode char(20),
    @foldername char(70),
    @docref char(9),
    @request_date char(8),
    @request_time char(6),
    @eventtype char(1),
    @description char(30),
    @volume char(20),
    @pagefile char(10),
    @doctype char(3),
    @filler char(3),
    @hderror char(1),
    @processed char(1),
    @create_date datetime
AS
BEGIN
    INSERT INTO DOC_history (
        task,
        cabinetcode,
        cabinetname,
        drawercode,
        drawername,
        foldercode,
        foldername,
        docref,
        request_date,
        request_time,
        eventtype,
        description,
        volume,
        pagefile,
        doctype,
        filler,
        hderror,
        processed,
        create_date)
    VALUES (
        @task,
        @cabinetcode,
        @cabinetname,
        @drawercode,
        @drawername,
        @foldercode,
        @foldername,
        @docref,
        @request_date,
        @request_time,
        @eventtype,
        @description,
        @volume,
        @pagefile,
        @doctype,
        @filler,
        @hderror,
        @processed,
        @create_date)

    SELECT @history_id = @@IDENTITY
END
GO

