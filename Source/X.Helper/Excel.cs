using Microsoft.VisualBasic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper
{
    public static class Excel
    {

        /// <summary>
        /// 从指定EXCEL附件获取DATATABLE
        /// </summary>
        /// <param name="file">EXCEL文件</param>
        /// <param name="rows">返回行数 0为不限</param>
        /// <param name="startRow">开始行数</param>
        /// <returns></returns>
        public static System.Data.DataTable GetDataTable(FileInfo file, int rows = 0, int startRow = 0)
        {
            try
            {
                System.Data.DataTable dt;
                dt = new System.Data.DataTable();
                if (!file.Exists)
                    throw new FileNotFoundException("文件不存在！", file.FullName);
                using (FileStream fsRead = file.OpenRead())
                {
                    IWorkbook wk = null;
                    //获取后缀名
                    string ext = file.Extension.ToLower();
                    //判断是否是excel文件
                    if (ext == ".xlsx" || ext == ".xls")
                    {
                        //判断excel的版本
                        if (ext == ".xlsx")
                        {
                            wk = new XSSFWorkbook(fsRead);
                        }
                        else
                        {
                            wk = new HSSFWorkbook(fsRead);
                        }
                        //获取第一个sheet
                        ISheet sheet = wk.GetSheetAt(0);
                        if (null == sheet)
                            return null;
                        //获取第一行
                        IRow headrow = sheet.GetRow(startRow);
                        if (null == headrow)
                            return null;
                        //创建列
                        for (int i = headrow.FirstCellNum; i < headrow.Cells.Count; i++)
                        {
                            DataColumn datacolum = new DataColumn("F" + (i + 1));
                            dt.Columns.Add(datacolum);
                        }
                        //读取每行
                        for (int r = 0; r <= sheet.LastRowNum; r++)
                        {
                            bool result = false;
                            DataRow dr = dt.NewRow();
                            //获取当前行
                            IRow row = sheet.GetRow(r);
                            //如果当前行数据为空则跳过
                            if (null == row)
                                continue;
                            int colCount = row.Cells.Count;
                            //以第一行所在的列数为准
                            if (colCount != headrow.Cells.Count)
                            {
                                colCount = headrow.Cells.Count;
                            }
                            //读取每列
                            for (int j = 0; j < colCount; j++)
                            {
                                ICell cell = row.GetCell(j); //一个单元格
                                if (null == cell)
                                    continue;
                                cell.SetCellType(CellType.String);
                                if (string.IsNullOrEmpty(cell.StringCellValue))
                                    continue;
                                dr[j] = GetCellValue(cell); //获取单元格的值
                                                            //全为空则不取
                                if (dr[j].ToString() != "")
                                {
                                    result = true;
                                }
                            }
                            if (result == true)
                            {
                                dt.Rows.Add(dr); //把每行追加到DataTable
                            }
                        }
                    }

                }


                if (rows > 0 && dt.Rows.Count > rows)
                {
                    dt = dt.AsEnumerable().Take(rows).CopyToDataTable();
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取excel内容
        /// </summary>
        /// <param name="path">EXCEL文件路径</param>
        /// <param name="sheetNumber">SHEET索引号</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string path, int sheetNumber = 0)
        {
            //var attachment = Core.DbFactory.Common.Attachment.QueryByPrimaryKey(fileId);
            var filePath = path;
            DataTable dt = new DataTable();
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在！", filePath);
            using (FileStream fsRead = System.IO.File.OpenRead(filePath))
            {
                IWorkbook wk = null;
                //获取后缀名
                string ext = filePath.Substring(filePath.LastIndexOf(".")).ToString().ToLower();
                //判断是否是excel文件
                if (ext == ".xlsx" || ext == ".xls")
                {
                    //判断excel的版本
                    if (ext == ".xlsx")
                    {
                        wk = new XSSFWorkbook(fsRead);
                    }
                    else
                    {
                        wk = new HSSFWorkbook(fsRead);
                    }
                    //获取指定索引的SHEET
                    ISheet sheet = wk.GetSheetAt(sheetNumber);
                    if (null == sheet)
                        return null;
                    //获取第一行
                    IRow headrow = sheet.GetRow(0);
                    if (null == headrow)
                        return null;
                    //创建列
                    for (int i = headrow.FirstCellNum; i < headrow.Cells.Count; i++)
                    {
                        DataColumn datacolum = new DataColumn("F" + (i + 1));
                        dt.Columns.Add(datacolum);
                    }
                    //读取每行
                    for (int r = 0; r <= sheet.LastRowNum; r++)
                    {
                        bool result = false;
                        DataRow dr = dt.NewRow();
                        //获取当前行
                        IRow row = sheet.GetRow(r);
                        //如果当前行数据为空则跳过
                        if (null == row)
                            continue;
                        int colCount = row.Cells.Count;
                        //以第一行所在的列数为准
                        if (colCount > headrow.Cells.Count)
                        {
                            colCount = headrow.Cells.Count;
                        }
                        //读取每列
                        for (int j = 0; j < colCount; j++)
                        {
                            ICell cell = row.GetCell(j); //一个单元格
                            dr[j] = GetCellValue(cell); //获取单元格的值
                                                        //全为空则不取
                            if (dr[j].ToString() != "")
                            {
                                result = true;
                            }
                        }
                        if (result == true)
                        {
                            dt.Rows.Add(dr); //把每行追加到DataTable
                        }
                    }
                }

            }
            return dt;
        }

        //对单元格进行判断取值
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank: //空数据类型 
                    return string.Empty;
                case CellType.Boolean: //bool类型
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric: //数字类型
                    if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
                    {
                        return cell.DateCellValue.ToString();
                    }
                    else //其它数字
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Unknown: //无法识别类型
                default: //默认类型
                    return cell.ToString();//
                case CellType.String: //string 类型
                    return cell.StringCellValue;
                case CellType.Formula: //带公式类型
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

    }
}
