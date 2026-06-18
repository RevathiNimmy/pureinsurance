EXECUTE DDLDropProcedure 'spu_add_user'
GO

CREATE PROCEDURE spu_add_user
	@language_id int,
	@username varchar(255),
	@password varchar(100),
	@password_change_date datetime,
	@date_created datetime,
	@lastlogin datetime,
	@party_cnt int = NULL,
	@logged_on_at_client varchar(100),
	@is_pmb_link_required smallint,
	@server_printer varchar(10) = null,
	@is_printer_changeable smallint,
	@is_deleted smallint,
	@effective_date datetime
AS
BEGIN

declare @user_id int

	select @user_id = isnull(max(user_id)+1, 1) from pmuser

	insert into pmuser
		(
		user_id,
		language_id,
		username,
		password,
		password_change_date,
		date_created,
		lastlogin,
		party_cnt,
		is_pmb_link_required,
		logged_on_at_client,
		server_printer,
		is_printer_changeable,
		is_deleted,
		effective_date
		)
	values
		(
		@user_id,
		@language_id,
		@username,
		@password,
		@password_change_date,
		@date_created,
		@lastlogin,
		@party_cnt,
		@is_pmb_link_required,
		@logged_on_at_client,
		@server_printer,
		@is_printer_changeable,
		@is_deleted,
		@effective_date
		)
		
END

GO