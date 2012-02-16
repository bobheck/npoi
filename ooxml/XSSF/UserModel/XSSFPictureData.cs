/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.OpenXml4Net.OPC;
using NPOI.Util;
using System.IO;
using System;
namespace NPOI.XSSF.UserModel
{

    /**
     * Raw picture data, normally attached to a SpreadsheetML Drawing.
     * As a rule, pictures are stored in the /xl/media/ part of a SpreadsheetML package.
     */
    public class XSSFPictureData : POIXMLDocumentPart, IPictureData
    {

        /**
         * Relationships for each known picture type
         */
        internal static POIXMLRelation[] RELATIONS;
        static XSSFPictureData()
        {
            RELATIONS = new POIXMLRelation[8];
            RELATIONS[(int)PictureType.EMF] = XSSFRelation.IMAGE_EMF;
            RELATIONS[(int)PictureType.WMF] = XSSFRelation.IMAGE_WMF;
            RELATIONS[(int)PictureType.PICT] = XSSFRelation.IMAGE_PICT;
            RELATIONS[(int)PictureType.JPEG] = XSSFRelation.IMAGE_JPEG;
            RELATIONS[(int)PictureType.PNG] = XSSFRelation.IMAGE_PNG;
            RELATIONS[(int)PictureType.DIB] = XSSFRelation.IMAGE_DIB;
        }

        /**
         * Create a new XSSFPictureData node
         *
         * @see NPOI.xssf.usermodel.XSSFWorkbook#AddPicture(byte[], int)
         */
        protected XSSFPictureData()
            : base()
        {

        }

        /**
         * Construct XSSFPictureData from a namespace part
         *
         * @param part the namespace part holding the Drawing data,
         * @param rel  the namespace relationship holding this Drawing,
         * the relationship type must be http://schemas.Openxmlformats.org/officeDocument/2006/relationships/image
         */
        public XSSFPictureData(PackagePart part, PackageRelationship rel)
            : base(part, rel)
        {

        }

        /**
         * Gets the picture data as a byte array.
         * <p>
         * Note, that this call might be expensive since all the picture data is copied into a temporary byte array.
         * You can grab the picture data directly from the underlying namespace part as follows:
         * <br/>
         * <code>
         * InputStream is = GetPackagePart().GetInputStream();
         * </code>
         * </p>
         *
         * @return the picture data.
         */
        public byte[] GetData()
        {
                try
                {
                    return IOUtils.ToByteArray(GetPackagePart().GetInputStream());
                }
                catch (IOException e)
                {
                    throw new POIXMLException(e);
                }
        }

        /**
         * Suggests a file extension for this image.
         *
         * @return the file extension.
         */
        public String SuggestFileExtension()
        {
            return GetPackagePart().PartName.Extension;
        }

        /**
         * Return an integer constant that specifies type of this picture
         *
         * @return an integer constant that specifies type of this picture 
         * @see NPOI.ss.usermodel.Workbook#PICTURE_TYPE_EMF
         * @see NPOI.ss.usermodel.Workbook#PICTURE_TYPE_WMF
         * @see NPOI.ss.usermodel.Workbook#PICTURE_TYPE_PICT
         * @see NPOI.ss.usermodel.Workbook#PICTURE_TYPE_JPEG
         * @see NPOI.ss.usermodel.Workbook#PICTURE_TYPE_PNG
         * @see NPOI.ss.usermodel.Workbook#PICTURE_TYPE_DIB
         */
        public int GetPictureType()
        {
            String contentType = GetPackagePart().ContentType;
            for (int i = 0; i < RELATIONS.Length; i++)
            {
                if (RELATIONS[i] == null) continue;

                if (RELATIONS[i].ContentType.Equals(contentType))
                {
                    return i;
                }
            }
            return 0;
        }

        public String GetMimeType()
        {
            return GetPackagePart().ContentType;
        }
    }
}


