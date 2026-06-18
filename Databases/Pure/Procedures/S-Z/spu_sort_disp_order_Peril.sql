SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sort_disp_order_Peril'
GO

CREATE PROCEDURE spu_sort_disp_order_Peril
    @ChkOrder int,
    @dataid int
AS
DECLARE @Disp_Order int
DECLARE @Disp_id int
DECLARE @Peril_Type_id int

---Select @Peril_Type_id=Peril_type_id from Peril_data_definition where Peril_data_defn_id=@dataid

SELECT @Peril_Type_id = @Dataid

DECLARE DefineFields_cursor CURSOR FAST_FORWARD FOR
    SELECT Peril_data_defn_id, display_order
    FROM Peril_data_definition
    WHERE type <> 6
    AND Peril_type_id = @Peril_Type_id
    ORDER BY display_order, Peril_data_defn_id DESC

OPEN DefineFields_cursor
FETCH NEXT FROM DefineFields_cursor INTO @Disp_id, @Disp_Order

WHILE @@FETCH_STATUS = 0 BEGIN
    IF @ChkOrder = @Disp_Order BEGIN
        FETCH NEXT FROM DefineFields_cursor INTO @Disp_id, @Disp_Order

        WHILE @@FETCH_STATUS = 0 BEGIN
            --update the records from here on
            UPDATE Peril_data_definition
                SET display_order = @disp_order + 1
                WHERE Peril_data_defn_id = @Disp_id

            SELECT @disp_order = @disp_order + 1

            FETCH NEXT FROM DefineFields_cursor INTO @Disp_id, @Disp_Order
        END
    END ELSE BEGIN
        FETCH NEXT FROM DefineFields_cursor INTO @Disp_id, @Disp_Order
    END
END

CLOSE DefineFields_cursor
DEALLOCATE DefineFields_cursor

GO

