set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'Policy2Folders'
go
create view Policy2Folders as select
    Insurance_Folder.insurance_folder_cnt   ID,
    Insurance_Folder.insurance_holder_cnt   OwnerClientID,
    Insurance_Folder.code                   Number,
    Source.code                             BranchCode,
    Source.description                      BranchName,
    Insurance_Folder.inception_date         InceptionDate,
    Insurance_Folder.renewal_count          TimesRenewed
    from Insurance_Folder
    left outer join Source on Insurance_Folder.source_id = Source.source_id
go
