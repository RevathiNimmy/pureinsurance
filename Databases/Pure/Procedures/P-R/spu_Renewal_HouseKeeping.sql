SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Renewal_HouseKeeping'
GO

CREATE PROCEDURE spu_Renewal_HouseKeeping
AS
/* Description :
    1. select all records from renewal_control where status is LIVE OR CONFIRM('RENEWCONF') or LAPSE
    2. Then move on to Events and then pick all records of type 'Renewal' and delete them
       the status type id = 15
    3. From Insurance file pick all the file where insurance_file_status = 'Renewal Quote' for this folder
    4. Find out related Risk date for the Insurance and delete them

    NOTE : If insurance_file_status = NULL then it is LIVE policy that should not be deleted
           and all the insurance_file_status.code = 'REP' - values '4' should be deleted
   History : SSL 06042001
*/
BEGIN
    DECLARE @renewconf_id int
    DECLARE @lapse_id int
    DECLARE @event_status_id int

    DECLARE @insurance_folder_cnt int
    DECLARE @insurance_file_cnt int

    DECLARE @insurance_file_type_id int
    DECLARE @insurance_file_status_id int

    SELECT @renewconf_id = renewal_status_type_id
        FROM renewal_status_type
        WHERE code = 'RENEWCONF'

    SELECT @lapse_id = renewal_status_type_id
        FROM renewal_status_type
        WHERE code = 'LAPSECONF'

    SELECT @event_status_id = event_type_id
        FROM event_type
        WHERE code = 'RENEWAL'

    SELECT @insurance_file_type_id = insurance_file_type_id
        FROM insurance_file_type
        WHERE code = 'POLICY'

    SELECT @insurance_file_status_id = insurance_file_status_id
        FROM insurance_file_status
        WHERE code = 'REP'

    /*choose all records where renewal_status_type_id = RENEWCONF or LAPSED*/
    /* put all the selected records into cursors*/

    DECLARE cRenewalControl CURSOR FAST_FORWARD FOR
        SELECT insurance_folder_cnt
        FROM renewal_control
        WHERE renewal_status_type_id IN (@renewconf_id, @lapse_id)

    /* open the cursor cRenewalControl */
    OPEN cRenewalControl

    FETCH NEXT FROM cRenewalControl INTO @insurance_folder_cnt

    WHILE @@FETCH_STATUS = 0 BEGIN
        /*2. delete from event_log where event_type_id = RENEWAL*/
        DELETE FROM event_log
            WHERE insurance_folder_cnt = @insurance_folder_cnt
            AND event_type_id = @event_status_id

        /* select all records where insurance_file_status_id = 'REP' */
        DECLARE cInsuranceFile CURSOR FAST_FORWARD FOR
            SELECT insurance_file_cnt FROM insurance_file
            WHERE insurance_file_status_id = @insurance_file_status_id
            AND insurance_folder_cnt = @insurance_folder_cnt

        /* open cursor cInsuranceFile */
        OPEN cInsuranceFile
        FETCH NEXT FROM cInsuranceFile INTO @insurance_file_cnt

        WHILE @@FETCH_STATUS = 0 BEGIN
            /* finding out related risk details for the insurance_folder
               based on insurance_file_cnt */

            DECLARE @table_name varchar(70)
            DECLARE @policy_binder_id int
            DECLARE @sqlstring varchar(100)

            DECLARE cGISObjects CURSOR FAST_FORWARD FOR
            SELECT DISTINCT go.table_name, spb.sbo_policy_binder_id
                FROM gis_object go,
                gis_property gp,
                gis_policy_link gpl,
                sbo_policy_binder spb
                WHERE gpl.gis_policy_link_id = spb.gis_policy_link_id
                AND gpl.gis_data_model_id = go.gis_data_model_id
                AND go.gis_object_id = gp.gis_object_id
                AND gpl.insurance_file_cnt = @insurance_file_cnt

            /* open cursor cGISObjects*/
            OPEN cGISObjects
            FETCH NEXT FROM cGISObjects into @table_name, @policy_binder_id

            /* iterate through cursor records */
            WHILE @@FETCH_STATUS = 0 BEGIN
                /* 4. delete all the risk table information retrieved from GIS_Object table */
                SET @sqlstring = 'SELECT * FROM ' + @table_name +
                         ' WHERE sbo_policy_binder_id = ' + @policy_binder_id

                IF EXISTS (SELECT @sqlstring) BEGIN
                    SET @sqlstring = 'DELETE FROM ' + @table_name +
                             ' WHERE sbo_policy_binder_id = ' + @policy_binder_id
                    EXECUTE @sqlstring
                END

                FETCH NEXT FROM cGISObjects INTO @table_name, @policy_binder_id
            END

            CLOSE cGISObjects
            DEALLOCATE cGISObjects

            /* 3. delete all records from Insurance_file that matches @insurance_file_cnt */
            DELETE FROM Insurance_File WHERE insurance_file_cnt = @insurance_file_cnt

            FETCH NEXT FROM cInsuranceFile INTO @insurance_file_cnt
        END

        /* close the cursor */
        CLOSE cInsuranceFile

        /* deallocote the cursor from memory*/
        DEALLOCATE cInsuranceFile

        /*1. delete records from renewal_control where renewal_status is 'LAPSED' */
        DELETE FROM renewal_control
            WHERE insurance_folder_cnt = @insurance_folder_cnt

        FETCH NEXT FROM cRenewalControl INTO @insurance_folder_cnt
    END

    /* Close the cursor */
    CLOSE cRenewalControl

    /* Remove it from memory */
    DEALLOCATE cRenewalControl
END
GO

