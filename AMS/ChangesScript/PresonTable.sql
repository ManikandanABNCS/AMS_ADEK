Alter Table PersonTable add CreatedBy int null foreign key references user_loginusertable(userID)
go 
Alter table PersonTable add CreatedDateTime SmallDatetime NULL
go 

Alter Table PersonTable add LastModifiedBy int null foreign key references user_loginusertable(userID)
go 
Alter table PersonTable add LastModifiedDateTime SmallDatetime NULL
go
update a set 
a.CreatedDateTime=b.CreatedDate,
a.CreatedBy=1 
from PersonTable a 
join User_LoginUserTable b on a.PersonID=b.UserID
go 

Alter Table PersonTable Alter column CreatedBy int not null 
go 

Alter Table PersonTable Alter column CreatedDateTime smalldatetime not null 
go 