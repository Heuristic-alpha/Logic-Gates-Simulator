using System;
using System.Collections.Generic;
using System.Text;

namespace HSCL
{
    public class XmlParser
    {
        const char OpenAngleBracket = '<';
        const char CloseAngleBracket = '>';
        const char SlashSymbol = '/';
        const char EqualChar = '=';
        const char DoubleQuotes = '\"';
        const string Indent = "    ";

        private char[] m_sourcArray;
        public List<XmlTagInfo> m_listOfTags = new List<XmlTagInfo>();
        public XmlTagInfo RootNode { get; private set; } = null;

        public char[] SourceArray { get { return m_sourcArray; } }

        /// <summary>
        /// for Complete parsing
        /// </summary>
        /// <param name="source"> the source string that contain xml data </param>
        public XmlParser(string source)
        {
            m_sourcArray = PrepareString(source);
            CompleteParserStage();
            ParrentingStage();
        }
        /// <summary>
        /// for Slight parsing
        /// </summary>
        /// <param name="source"> the source string that contain xml data </param>
        /// <param name="wantedTag"> parse until found this wantedTag </param>
        public XmlParser(string source, string wantedTag)
        {
            m_sourcArray = PrepareString(source);
            SlightParserStage(wantedTag);
            ParrentingStage();
        }
        private enum ParserStageState
        {
            None,
            SkippingWhiteSpace,
            FindingOppeningTag,
            FindingClosingTag,
            FindingAttributeName,
            FindingAttributeValue,
        }
        private char[] PrepareString(string inputString)
        {
            inputString += " ";
            var chars = inputString.ToCharArray();
            var len = chars.Length;
            //// convert any White space to space char:
            //for (int i = 0; i < len; i++) if (char.IsWhiteSpace(chars[i])) chars[i] = ' ';
            return chars;
        }
        /// <summary>
        /// process the entire charArray and insert founded tags to m_listOfTags:                
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void CompleteParserStage()
        {
            int tempIndex = -1; // -1 means nothing
            ParserStageState currentState = ParserStageState.None;
            ParserStageState previousState = ParserStageState.None;
            XmlTagInfo currentTag = null;
            bool isReachedFirstDoubleQuotes = false;
            string temp_Attribute_Name = string.Empty;
            char currentChar = '\0';
            char nextChar = '\0';


            // before start the loop
            ChangeState(ParserStageState.None);
            // main for loop:
            for (int currentIndex = 0; currentIndex < m_sourcArray.Length - 1; currentIndex++)
            {
                currentChar = m_sourcArray[currentIndex];
                nextChar = SourceArray[currentIndex + 1];

                switch (currentState)
                {
                    case ParserStageState.None:
                        {
                            if (currentChar == OpenAngleBracket) // '<'
                            {
                                if (nextChar == SlashSymbol) // it is clossing Tag  // '</'
                                {
                                    tempIndex = currentIndex;
                                    ChangeState(ParserStageState.FindingClosingTag);
                                }
                                else if (nextChar == OpenAngleBracket || nextChar == CloseAngleBracket)
                                {
                                    throw new Exception("Error: we shouldnt reach here (1) !!!");
                                }
                                else // it is a openning Tag   // '<'
                                {
                                    tempIndex = currentIndex;
                                    ChangeState(ParserStageState.FindingOppeningTag);
                                }
                            }
                        }
                        break;
                    case ParserStageState.SkippingWhiteSpace: // skipped white space inside inner tag body until reached at the end of white characters
                        {
                            if (char.IsWhiteSpace(currentChar) && !char.IsWhiteSpace(nextChar)) // reached end of white chars
                            {
                                if (nextChar == CloseAngleBracket) // reached to '>' 
                                {
                                    if (currentTag == null) throw new Exception("Error (1) currentTag here should not be NULL !");
                                    currentTag.SetCloseAngleBracketIndex_OfStartTag(currentIndex + 1);
                                    ChangeState(ParserStageState.None);
                                }
                                else if (nextChar == SlashSymbol && m_sourcArray[currentIndex + 2] == CloseAngleBracket) // reached to '/>' so it is selfClossingTag
                                {
                                    if (previousState == ParserStageState.FindingOppeningTag ||
                                       previousState == ParserStageState.FindingAttributeValue)
                                    {
                                        if (currentTag == null) throw new Exception("Error (3) currentTag here should not be NULL !");
                                        currentTag.IsSelfClosing = true;
                                        currentTag.SetEndTagIndex(currentIndex + 1);
                                        ChangeState(ParserStageState.None);
                                    }
                                    else
                                    {
                                        throw new Exception("Error: we shouldn't reach here (3) !!!");
                                    }
                                }
                                else // reached to everything else so there is another attribute:
                                {
                                    if (previousState == ParserStageState.FindingOppeningTag ||
                                       previousState == ParserStageState.FindingAttributeValue)
                                    {
                                        tempIndex = currentIndex;
                                        ChangeState(ParserStageState.FindingAttributeName);
                                    }
                                    else
                                    {
                                        throw new Exception("Error: we shouldn't reach here (2) !!!");
                                    }
                                }
                            }

                        }
                        break;
                    case ParserStageState.FindingOppeningTag:
                        {
                            if (currentChar == CloseAngleBracket) // '>'
                            {
                                // successfuly found a oppening Tag:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                string tag = new string(subArray);

                                // add the current founded Tag to the List:
                                var foundedTag = new XmlTagInfo(m_sourcArray, tag, tempIndex);
                                foundedTag.SetCloseAngleBracketIndex_OfStartTag(currentIndex);
                                m_listOfTags.Add(foundedTag);
                                currentTag = foundedTag;
                                ChangeState(ParserStageState.None);
                            }
                            else if (currentChar == OpenAngleBracket ||
                                     currentChar == DoubleQuotes) // '<'  '"'
                            {
                                throw new Exception($"XmlParser.CompleteParserStage() Error: currentChar( {currentChar} ) in currentState( {currentState} ) cant be \'<\'  or '\"' .");
                            }
                            else if (currentChar == SlashSymbol && nextChar == CloseAngleBracket) // reached to '/>' so it is selfClossingTag
                            {
                                // successfuly found a SelfClosing Tag:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                string tag = new string(subArray);

                                // add the current founded Tag to the List:
                                var foundedTag = new XmlTagInfo(m_sourcArray, tag, tempIndex);
                                m_listOfTags.Add(foundedTag);
                                currentTag = foundedTag;
                                currentTag.IsSelfClosing = true;
                                currentTag.SetEndTagIndex(currentIndex + 1);
                                ChangeState(ParserStageState.None);
                            }
                            else if (char.IsWhiteSpace(currentChar))
                            {
                                // successfuly found a oppening Tag and next is White space:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                string tag = new string(subArray);

                                // add the current founded Tag to the List:
                                var foundedTag = new XmlTagInfo(m_sourcArray, tag, tempIndex);
                                m_listOfTags.Add(foundedTag);
                                currentTag = foundedTag;
                                currentIndex--; // !!! Attention: if we dont reduce current index then we cant change states properly
                                ChangeState(ParserStageState.SkippingWhiteSpace);
                            }
                        }
                        break;
                    case ParserStageState.FindingClosingTag:
                        {
                            if (currentChar == CloseAngleBracket)
                            {
                                // successfuly found a clossing Tag:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 2, currentIndex - 1);
                                string tag = new string(subArray);
                                // found the current none complete Tag in the list and complete it:
                                bool tagIsFound = false;
                                foreach (XmlTagInfo tagInfo in m_listOfTags)
                                {
                                    if (tagInfo.IsComplete() || tagInfo.IsSelfClosing) continue;
                                    else if (tagInfo.Tag.Equals(tag))
                                    {
                                        tagInfo.SetEndTagIndex(currentIndex);
                                        ChangeState(ParserStageState.None);
                                        tagIsFound = true;
                                        break;
                                    }
                                }
                                // if nothing found in previus foreach loop so there is an error:
                                if (tagIsFound == false)
                                {
                                    throw new Exception($"XmlParser.CompleteParserStage() Error: Cant found any matched Tag with {tag} .");
                                }
                            }
                        }
                        break;
                    case ParserStageState.FindingAttributeName:
                        {
                            if (currentChar == EqualChar)
                            {
                                // successfuly found a Attribute Name:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                temp_Attribute_Name = new string(subArray);

                                ChangeState(ParserStageState.FindingAttributeValue);
                            }
                        }
                        break;
                    case ParserStageState.FindingAttributeValue:
                        {
                            if (currentChar == DoubleQuotes)
                            {
                                if (isReachedFirstDoubleQuotes == false)
                                {
                                    tempIndex = currentIndex;
                                    isReachedFirstDoubleQuotes = true;
                                }
                                else // true so it is second DoubleQuotes
                                {
                                    // successfuly found a Attribute Value:
                                    char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                    string value = new string(subArray);

                                    XmlAttributeInfo xmlAttribute = new(temp_Attribute_Name, value);
                                    if (currentTag == null) throw new Exception("Error (2) currentTag here should not be NULL !");
                                    currentTag.AddAttributeInfo(xmlAttribute);

                                    if (char.IsWhiteSpace(nextChar)) //    ' ' 
                                    {
                                        ChangeState(ParserStageState.SkippingWhiteSpace);
                                    }
                                    else if (nextChar == CloseAngleBracket) //     '>'
                                    {
                                        currentTag.SetCloseAngleBracketIndex_OfStartTag(currentIndex + 1);
                                        ChangeState(ParserStageState.None);
                                    }
                                    else if (nextChar == SlashSymbol && SourceArray[currentIndex + 2] == CloseAngleBracket) // reached to '/>' so it is selfClossingTag
                                    {
                                        if (currentTag == null) throw new Exception("Error (3) currentTag here should not be NULL !");
                                        currentTag.IsSelfClosing = true;
                                        currentTag.SetEndTagIndex(currentIndex + 2);
                                        ChangeState(ParserStageState.None);
                                    }
                                    else throw new Exception("Error: we shouldnt reach here (4) !!!");
                                }
                            }
                        }
                        break;
                }


            }//end of main loop

            //end of method.

            //local method:
            void ChangeState(ParserStageState newState)
            {
                previousState = currentState;
                currentState = newState;
                switch (newState)
                {
                    case ParserStageState.None:
                        {
                            tempIndex = -1;
                            isReachedFirstDoubleQuotes = false;
                            temp_Attribute_Name = string.Empty;
                            currentTag = null;
                        }
                        break;
                    case ParserStageState.SkippingWhiteSpace:
                        {
                            tempIndex = -1;
                            isReachedFirstDoubleQuotes = false;
                        }
                        break;
                    case ParserStageState.FindingAttributeName:
                        {
                            isReachedFirstDoubleQuotes = false;
                            temp_Attribute_Name = string.Empty;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// process the charArray until found most wanted wantedTag and insert founded tags to m_listOfTags:        
        /// </summary>
        /// <param name="wantedTag"></param>
        /// <exception cref="Exception"></exception>
        private void SlightParserStage(string wantedTag)
        {
            bool continueParsing = true;
            int tempIndex = -1; // -1 means nothing
            ParserStageState currentState = ParserStageState.None;
            ParserStageState previousState = ParserStageState.None;
            XmlTagInfo currentTag = null;
            bool isReachedFirstDoubleQuotes = false;
            string temp_Attribute_Name = string.Empty;
            char currentChar = '\0';
            char nextChar = '\0';


            // before start the loop
            ChangeState(ParserStageState.None);
            // main for loop:
            for (int currentIndex = 0; (currentIndex < m_sourcArray.Length - 1) && continueParsing; currentIndex++)
            {
                currentChar = m_sourcArray[currentIndex];
                nextChar = SourceArray[currentIndex + 1];

                switch (currentState)
                {
                    case ParserStageState.None:
                        {
                            if (currentChar == OpenAngleBracket) // '<'
                            {
                                if (nextChar == SlashSymbol) // it is clossing Tag  // '</'
                                {
                                    tempIndex = currentIndex;
                                    ChangeState(ParserStageState.FindingClosingTag);
                                }
                                else if (nextChar == OpenAngleBracket || nextChar == CloseAngleBracket)
                                {
                                    throw new Exception("Error: we shouldnt reach here (1) !!!");
                                }
                                else // it is a openning Tag   // '<'
                                {
                                    tempIndex = currentIndex;
                                    ChangeState(ParserStageState.FindingOppeningTag);
                                }
                            }
                        }
                        break;
                    case ParserStageState.SkippingWhiteSpace: // skipped white space inside inner tag body until reached at the end of white characters
                        {
                            if (char.IsWhiteSpace(currentChar) && !char.IsWhiteSpace(nextChar)) // reached end of white chars
                            {
                                if (nextChar == CloseAngleBracket) // reached to '>' 
                                {
                                    if (currentTag == null) throw new Exception("Error (1) currentTag here should not be NULL !");
                                    currentTag.SetCloseAngleBracketIndex_OfStartTag(currentIndex + 1);
                                    ChangeState(ParserStageState.None);
                                }
                                else if (nextChar == SlashSymbol && m_sourcArray[currentIndex + 2] == CloseAngleBracket) // reached to '/>' so it is selfClossingTag
                                {
                                    if (previousState == ParserStageState.FindingOppeningTag ||
                                       previousState == ParserStageState.FindingAttributeValue)
                                    {
                                        if (currentTag == null) throw new Exception("Error (3) currentTag here should not be NULL !");
                                        currentTag.IsSelfClosing = true;
                                        currentTag.SetEndTagIndex(currentIndex + 1);
                                        ChangeState(ParserStageState.None);
                                    }
                                    else
                                    {
                                        throw new Exception("Error: we shouldn't reach here (3) !!!");
                                    }
                                }
                                else // reached to everything else so there is another attribute:
                                {
                                    if (previousState == ParserStageState.FindingOppeningTag ||
                                       previousState == ParserStageState.FindingAttributeValue)
                                    {
                                        tempIndex = currentIndex;
                                        ChangeState(ParserStageState.FindingAttributeName);
                                    }
                                    else
                                    {
                                        throw new Exception("Error: we shouldn't reach here (2) !!!");
                                    }
                                }
                            }

                        }
                        break;
                    case ParserStageState.FindingOppeningTag:
                        {
                            if (currentChar == CloseAngleBracket) // '>'
                            {
                                // successfuly found a oppening Tag:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                string tag = new string(subArray);

                                // add the current founded Tag to the List:
                                var foundedTag = new XmlTagInfo(m_sourcArray, tag, tempIndex);
                                foundedTag.SetCloseAngleBracketIndex_OfStartTag(currentIndex);
                                m_listOfTags.Add(foundedTag);
                                currentTag = foundedTag;
                                ChangeState(ParserStageState.None);
                            }
                            else if (currentChar == OpenAngleBracket ||
                                     currentChar == DoubleQuotes) // '<'  '"'
                            {
                                throw new Exception($"XmlParser.SlightParserStage() Error: currentChar( {currentChar} ) in currentState( {currentState} ) cant be \'<\'  or '\"' .");
                            }
                            else if (currentChar == SlashSymbol && nextChar == CloseAngleBracket) // reached to '/>' so it is selfClossingTag
                            {
                                // successfuly found a SelfClosing Tag:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                string tag = new string(subArray);

                                // add the current founded Tag to the List:
                                var foundedTag = new XmlTagInfo(m_sourcArray, tag, tempIndex);
                                m_listOfTags.Add(foundedTag);
                                currentTag = foundedTag;
                                currentTag.IsSelfClosing = true;
                                currentTag.SetEndTagIndex(currentIndex + 1);
                                ChangeState(ParserStageState.None);
                            }
                            else if (char.IsWhiteSpace(currentChar))
                            {
                                // successfuly found a oppening Tag and next is White space:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                string tag = new string(subArray);

                                // add the current founded Tag to the List:
                                var foundedTag = new XmlTagInfo(m_sourcArray, tag, tempIndex);
                                m_listOfTags.Add(foundedTag);
                                currentTag = foundedTag;
                                currentIndex--; // !!! Attention: if we dont reduce current index then we cant change states properly
                                ChangeState(ParserStageState.SkippingWhiteSpace);
                            }
                        }
                        break;
                    case ParserStageState.FindingClosingTag:
                        {
                            if (currentChar == CloseAngleBracket)
                            {
                                // successfuly found a clossing Tag:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 2, currentIndex - 1);
                                string tag = new string(subArray);
                                // found the current none complete Tag in the list and complete it:
                                bool tagIsFound = false;
                                foreach (XmlTagInfo tagInfo in m_listOfTags)
                                {
                                    if (tagInfo.IsComplete() || tagInfo.IsSelfClosing) continue;
                                    else if (tagInfo.Tag.Equals(tag))
                                    {
                                        tagInfo.SetEndTagIndex(currentIndex);
                                        ChangeState(ParserStageState.None);
                                        tagIsFound = true;

                                        // check if current founded closing Tag is equal to wnatedTag then stop parsing:
                                        if (wantedTag.Equals(tag)) continueParsing = false;
                                        break;
                                    }
                                }
                                // if nothing found in previus foreach loop so there is an error:
                                if (tagIsFound == false)
                                {
                                    throw new Exception($"XmlParser.SlightParserStage() Error: Cant found any matched Tag with {tag} .");
                                }
                            }
                        }
                        break;
                    case ParserStageState.FindingAttributeName:
                        {
                            if (currentChar == EqualChar)
                            {
                                // successfuly found a Attribute Name:
                                char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                temp_Attribute_Name = new string(subArray);

                                ChangeState(ParserStageState.FindingAttributeValue);
                            }
                        }
                        break;
                    case ParserStageState.FindingAttributeValue:
                        {
                            if (currentChar == DoubleQuotes)
                            {
                                if (isReachedFirstDoubleQuotes == false)
                                {
                                    tempIndex = currentIndex;
                                    isReachedFirstDoubleQuotes = true;
                                }
                                else // true so it is second DoubleQuotes
                                {
                                    // successfuly found a Attribute Value:
                                    char[] subArray = m_sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                                    string value = new string(subArray);

                                    XmlAttributeInfo xmlAttribute = new(temp_Attribute_Name, value);
                                    if (currentTag == null) throw new Exception("Error (2) currentTag here should not be NULL !");
                                    currentTag.AddAttributeInfo(xmlAttribute);

                                    if (char.IsWhiteSpace(nextChar)) //    ' ' 
                                    {
                                        ChangeState(ParserStageState.SkippingWhiteSpace);
                                    }
                                    else if (nextChar == CloseAngleBracket) //     '>'
                                    {
                                        currentTag.SetCloseAngleBracketIndex_OfStartTag(currentIndex + 1);
                                        ChangeState(ParserStageState.None);
                                    }
                                    else if (nextChar == SlashSymbol && SourceArray[currentIndex + 2] == CloseAngleBracket) // reached to '/>' so it is selfClossingTag
                                    {
                                        if (currentTag == null) throw new Exception("Error (3) currentTag here should not be NULL !");
                                        currentTag.IsSelfClosing = true;
                                        currentTag.SetEndTagIndex(currentIndex + 2);
                                        ChangeState(ParserStageState.None);
                                    }
                                    else throw new Exception("Error: we shouldnt reach here (4) !!!");
                                }
                            }
                        }
                        break;
                }


            }//end of main loop

            //end of method.

            //local method:
            void ChangeState(ParserStageState newState)
            {
                previousState = currentState;
                currentState = newState;
                switch (newState)
                {
                    case ParserStageState.None:
                        {
                            tempIndex = -1;
                            isReachedFirstDoubleQuotes = false;
                            temp_Attribute_Name = string.Empty;
                            currentTag = null;
                        }
                        break;
                    case ParserStageState.SkippingWhiteSpace:
                        {
                            tempIndex = -1;
                            isReachedFirstDoubleQuotes = false;
                        }
                        break;
                    case ParserStageState.FindingAttributeName:
                        {
                            isReachedFirstDoubleQuotes = false;
                            temp_Attribute_Name = string.Empty;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// set nodes relationship together:
        /// </summary>
        private void ParrentingStage()
        {
            // the first node is always the root:
            RootNode = m_listOfTags[0];

            for (int i = 0; i < m_listOfTags.Count; i++)
            {
                XmlTagInfo currentNode = m_listOfTags[i];
                if (currentNode.IsSelfClosing) continue; // if currentNode is selfClosing so it hasn't any children then skip it!
                for (int j = i + 1; j < m_listOfTags.Count; j++)
                {
                    XmlTagInfo currentfoundedNode = m_listOfTags[j];
                    if (currentfoundedNode.IsInChildTreeOfNode(currentNode))
                    {
                        currentNode.AddChildNode(currentfoundedNode);
                        currentfoundedNode.ParrentNode = currentNode;
                        //check if current added child node is also in children node of it parrent then remove it from child node of parrent node.
                        if (currentNode.ParrentNode != null)
                        {
                            var listOfRemoveIndex = new List<int>();
                            XmlTagInfo[] childrenOfParrent = currentNode.ParrentNode.GetAllChildren();
                            for (int k = 0; k < childrenOfParrent.Length; k++)
                            {
                                // we cant directly remove element in ListOfChildrenNodes so we store the index of that element then remove it later:
                                XmlTagInfo childNode = childrenOfParrent[k];
                                if (ReferenceEquals(childNode, currentfoundedNode)) listOfRemoveIndex.Add(k);
                            }
                            // Remove phase:
                            foreach (int index in listOfRemoveIndex) currentNode.ParrentNode.ListOfChildrenNodes.RemoveAt(index);
                        }
                    }
                    //for Debugging:
                    // Console.WriteLine($"ParrentingStage::   i:({i}) j:({j}) currentNode.childCount:({currentNode.GetChildCount()})");
                }
            }
        }

        public bool TryGetContentFromTag(string tag, out string content)
        {
            // find the first matched wantedTag in the list and return it content
            foreach (XmlTagInfo currentNode in m_listOfTags)
            {
                if (tag.Equals(currentNode.Tag))
                {
                    content = currentNode.GetContent();
                    return true;
                }
            }
            content = string.Empty;
            return false;
        }
        public bool TryGetXMLTagInfoFromTag(string tag, out XmlTagInfo tagInfo)
        {
            // find the first matched wantedTag in the list and return it object
            foreach (XmlTagInfo currentNode in m_listOfTags)
            {
                if (tag.Equals(currentNode.Tag))
                {
                    tagInfo = currentNode;
                    return true;
                }
            }
            tagInfo = null;
            return false;
        }

        public static string ConvertToXmlFormat(TagType tagType, Format format, int indentLevel, string tagName, string tagContent)
        {
            if (format == Format.Inline)
            {
                if (tagType == TagType.SelfClosed)
                {
                    return $"<{tagName}/>";
                }
                else if (tagType == TagType.OpenClosed)
                {
                    return $"<{tagName}> {tagContent} </{tagContent}>";
                }
                else throw new NotImplementedException("ConvertToXmlFormat Error");
            }
            else if (format == Format.Indent)
            {
                if (tagType == TagType.SelfClosed)
                {
                    return MakeIndent(indentLevel) + $"<{tagName}/>";
                }
                else if (tagType == TagType.OpenClosed)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"<{tagName}>\n");
                    sb.Append(MakeIndent(indentLevel + 1)); sb.Append(tagContent + "\n");
                    sb.Append($"</{tagName}>\n");
                    return sb.ToString();
                }
                else throw new NotImplementedException("ConvertToXmlFormat Error");
            }
            else throw new NotImplementedException("ConvertToXmlFormat Error");
        }
        public static string ConvertToXmlFormat(TagType tagType, Format format, int indentLevel, string tagName, string tagContent, XmlAttributeInfo[] xmlAttributes)
        {
            if (format == Format.Inline)
            {
                if (tagType == TagType.SelfClosed)
                {
                    return $"<{tagName} {MakeAtrStr(xmlAttributes)}/>";
                }
                else if (tagType == TagType.OpenClosed)
                {
                    return $"<{tagName} {MakeAtrStr(xmlAttributes)}> {tagContent} </{tagContent}>";
                }
                else throw new NotImplementedException("ConvertToXmlFormat Error");
            }
            else if (format == Format.Indent)
            {
                if (tagType == TagType.SelfClosed)
                {
                    return MakeIndent(indentLevel) + $"<{tagName} {MakeAtrStr(xmlAttributes)}/>";
                }
                else if (tagType == TagType.OpenClosed)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"<{tagName} {MakeAtrStr(xmlAttributes)}>\n"); // open
                    sb.Append(MakeIndent(indentLevel + 1)); sb.Append(tagContent + "\n");
                    sb.Append($"</{tagName}>\n");// close
                    return sb.ToString();
                }
                else throw new NotImplementedException("ConvertToXmlFormat Error");
            }
            else throw new NotImplementedException("ConvertToXmlFormat Error");
        }
        private static string MakeIndent(int level)
        {
            string ret = "";
            for (int i = 1; i <= level; i++) ret += Indent;
            return ret;
        }
        private static string MakeAtrStr(XmlAttributeInfo[] xmlAttributes)
        {
            StringBuilder ret = new StringBuilder("");
            for (int i = 0; i < xmlAttributes.Length; i++)
            {
                ret.Append($"{xmlAttributes[i].Name}=\"{xmlAttributes[i].Value}\"");
                // cheks if there is another Attribute then add white space between them:
                if (i + 1 < xmlAttributes.Length) ret.Append(" ");
            }
            return ret.ToString();
        }
        public enum TagType
        {
            OpenClosed,
            SelfClosed,
        }
        public enum Format
        {
            Inline,
            Indent,
        }
    }

    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] data, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex + 1;
            T[] result = new T[length];
            Array.Copy(data, startIndex, result, 0, length);
            return result;
        }
    }

    public class XmlTagInfo
    {
        private readonly char[] sourcArray;
        private readonly string tag = "null";
        private int startTagIndex = -1;
        private int endTagIndex = -1;
        private int closeAngleBracketIndex_OfStartTag = -1; // we need this index for calculating length of startTag because it is dynamic
        private XmlTagInfo parrentNode = null;
        private List<XmlTagInfo> listOfChildren = new List<XmlTagInfo>();
        private List<XmlAttributeInfo> listOfAttributes = new List<XmlAttributeInfo>();

        public string Tag { get { return tag; } }
        public int StartTagIndex { get { return startTagIndex; } }
        public int EndTagIndex { get { return endTagIndex; } }
        public bool IsSelfClosing { get; set; }
        public XmlTagInfo ParrentNode { get { return parrentNode; } set { parrentNode = value; } }
        public List<XmlTagInfo> ListOfChildrenNodes { get { return listOfChildren; } }

        public XmlTagInfo(char[] sourcArray, string tag, int startTagIndex)
        {
            this.sourcArray = sourcArray;
            this.tag = tag;
            this.startTagIndex = startTagIndex;
        }

        public bool IsComplete() => (endTagIndex >= 0) ? true : false;
        public void SetEndTagIndex(int index) => endTagIndex = index;

        /// <summary>
        /// we need this index for calculating length of startTag because it is dynamic
        /// </summary>
        /// <param name="index"></param>
        public void SetCloseAngleBracketIndex_OfStartTag(int index) => closeAngleBracketIndex_OfStartTag = index;
        public override string ToString()
        {
            return $" (XmlTagInfo object) => Tag:({tag})  startIndex:({startTagIndex})  endIndex:({endTagIndex})";
        }
        public string GetContent()
        {
            if (IsSelfClosing) return string.Empty;
            else return new string(sourcArray.SubArray(closeAngleBracketIndex_OfStartTag + 1, endTagIndex - tag.Length - 3));
        }
        public void AddChildNode(XmlTagInfo node)
        {
            listOfChildren.Add(node);
        }
        public void RemoveChildNode(XmlTagInfo node)
        {
            listOfChildren.Remove(node);
        }
        public int GetChildCount() => listOfChildren.Count;
        public int GetAttributeCount() => listOfAttributes.Count;
        public bool IsInChildTreeOfNode(XmlTagInfo grandNode)
        {
            if (this.startTagIndex > grandNode.startTagIndex && this.endTagIndex < grandNode.endTagIndex) return true;
            else return false;
        }
        public XmlTagInfo GetChildNode(string tag)
        {
            // find the first matched wantedTag in the childrenList and return it object
            foreach (XmlTagInfo currentNode in ListOfChildrenNodes)
            {
                if (tag.Equals(currentNode.Tag))
                {
                    return currentNode;
                }
            }
            // if nothing found
            return null;
        }
        public XmlTagInfo[] GetAllChildren()
        {
            return listOfChildren.ToArray();
        }

        public void AddAttributeInfo(XmlAttributeInfo attribute)
        {
            listOfAttributes.Add(attribute);
        }
        public XmlAttributeInfo[] GetAllAttributes()
        {
            return listOfAttributes.ToArray();
        }
        public string GetValueOfAttribute(string attributeName)
        {
            foreach (XmlAttributeInfo attributeInfo in listOfAttributes)
            {
                if (attributeInfo.Name.Equals(attributeName)) return attributeInfo.Value;
            }
            return "[Error: NoValue!]";
        }
    }

    public class XmlAttributeInfo
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";

        public XmlAttributeInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}=\"{Value}\"";
        }
    }

} // end of HSCL namespace

