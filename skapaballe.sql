CREATE TABLE [dbo].[Tbl_DailyMenuHasDish] (
    [DMHD_Id]    INT IDENTITY (1, 1) NOT NULL,
    [DMHD_DM_Id] INT NOT NULL,
    [DMHD_Di_Id] INT NOT NULL,
    CONSTRAINT [PK_Tbl_DailyMenuHasDish] PRIMARY KEY CLUSTERED ([DMHD_Id] ASC),
    CONSTRAINT [FK_Tbl_DailyMenuHasDish_Tbl_DailyMenu] FOREIGN KEY ([DMHD_DM_Id]) REFERENCES [dbo].[Tbl_DailyMenu] ([DM_Id]),
    CONSTRAINT [FK_Tbl_DailyMenuHasDish_Tbl_Dish] FOREIGN KEY ([DMHD_Di_Id]) REFERENCES [dbo].[Tbl_Dish] ([Di_Id])
);
