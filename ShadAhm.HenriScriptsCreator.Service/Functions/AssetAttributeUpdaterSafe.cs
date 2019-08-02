using ShadAhm.HenriScriptsCreator.Service.Interfaces;
using ShadAhm.HenriScriptsCreator.Service.Models;
using ShadAhm.HenriScriptsCreator.Service.Models.Enums;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.IO;

namespace ShadAhm.HenriScriptsCreator.Service.Functions
{
    public class AssetAttributeUpdaterSafe : FunctionBaseAbstract
    {
        public AssetAttributeUpdaterSafe(DoFunctionSetting options) : base(options)
        {
        }

        public override IEnumerable<ConsoleMessage> Do()
        {
            yield return new ConsoleMessage(ConsoleMessageType.Okay, $"Begin. Is dry run : { dryRun }.");

            foreach (var sheetName in sheetNames)
            {
                Worksheet sheet = (Worksheet)excelUtil.Workbook.Sheets[sheetName];

                foreach (var message in DoSheet(sheet))
                {
                    yield return message; 
                }
            }
        }

        private IEnumerable<ConsoleMessage> DoSheet(Worksheet sheet)
        {
            string dir = $"{ this.outputDirPath }/ticket_{ this.ticketNo }_{ this.requesterName }";
            Directory.CreateDirectory(dir);

            Range excelRange = sheet.UsedRange;

            foreach (Range row in excelRange.Rows)
            {
                if (row.Row == 1) continue; // skip header row

                int excelRowNumber = row.Row;
                int saneRowNumber = row.Row - 2;

                int fileNumber = saneRowNumber / 2000;
                string filePath = CreateFile(dir, fileNumber);

                string assetTag = sheet.Cells[excelRowNumber, 1].Value2?.ToString().Trim();

                if (assetTag == "ENDMARKER") break;

                string assetAttributeName = sheet.Cells[excelRowNumber, 10].Value2?.ToString().Trim();
                string newValue = sheet.Cells[excelRowNumber, 9].Value2?.ToString().Trim();
                string newAssetAttributeId = sheet.Cells[excelRowNumber, 6].Value2?.ToString().Trim();
                int newAssetAttributeIdInt;

                if (!int.TryParse(newAssetAttributeId, out newAssetAttributeIdInt))
                {
                    yield return new ConsoleMessage(ConsoleMessageType.Error, $"New asset attribute id { newAssetAttributeId } not an int");
                    continue;
                }
                else
                {
                    yield return new ConsoleMessage(ConsoleMessageType.Okay, $"Doing Asset { assetTag }");
                }

                using (var tw = new StreamWriter(filePath, true))
                {
                    tw.WriteLine($"/** ROW: #{ excelRowNumber }; ASSET TAG: #{ assetTag } **/");
                    tw.WriteLine(WriteSqlIntoFile(assetTag, assetAttributeName, newValue, newAssetAttributeId));
                }
            }

            yield return new ConsoleMessage(ConsoleMessageType.Okay, $"**** DONE ****");
        }

        private string CreateFile(string dir, int fileNumber)
        {
            string filePath = $"{dir}/{this.ticketNo}_{fileNumber}.sql";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }

            return filePath;
        }

        private string WriteSqlIntoFile(string assetTag, string attributeDescription, string newValue, string assetAttributeId)
        {
            string updateAssetAttributeValue = $"EXEC [dbo].[AssetAttributeUpdate] @AssetTag = '{assetTag}', @AttributeDescription = '{attributeDescription}', @NewValue = '{newValue}', @AssetAttributeId = '{assetAttributeId}';";

            return updateAssetAttributeValue;
        }
    }
}
