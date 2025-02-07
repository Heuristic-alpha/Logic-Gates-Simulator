using System;
using System.Collections.Generic;

namespace HSCL
{
    public class XmlParser
    {
        const char OpenAngleBracket = '<';
        const char CloseAngleBracket = '>';
        const char SlashSymbol = '/';

        private char[] sourcArray;
        public List<XmlTagInfo> listOfTags = new List<XmlTagInfo>();
        public XmlTagInfo rootNode = null;

        public char[] SourceArray { get { return sourcArray; } }

        /// <summary>
        /// for Complete parsing
        /// </summary>
        /// <param name="source"> the source string that contain xml data </param>
        public XmlParser(string source)
        {
            sourcArray = source.ToCharArray();
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
            sourcArray = source.ToCharArray();
            SlightParserStage(wantedTag);
            ParrentingStage();
        }


        private enum ParserStageState
        {
            None,
            FindingOppeningTag,
            FindingClosingTag,
        }
        // process the entire charArray and insert founded tags to listOfTags:        
        private void CompleteParserStage()
        {
            int tempIndex = -1; // -1 means nothing
            ParserStageState state = ParserStageState.None;

            for (int currentIndex = 0; currentIndex < sourcArray.Length; currentIndex++)
            {
                if (sourcArray[currentIndex] == OpenAngleBracket) // currentIndex = '<'
                {
                    // if the last iteration is '<' so there is an error:                  
                    if (currentIndex == sourcArray.Length - 1)
                    {
                        throw new Exception($"XmlParser.CompleteParserStage() Error: Last index cant be \'<\' .");
                    }
                    else
                    {
                        if (sourcArray[currentIndex + 1] == SlashSymbol) // it is a clossing Tag
                        {
                            state = ParserStageState.FindingClosingTag;
                            tempIndex = currentIndex;
                        }
                        else if (sourcArray[currentIndex + 1] == CloseAngleBracket || sourcArray[currentIndex + 1] == OpenAngleBracket)
                        {
                            throw new Exception($"XmlParser.CompleteParserStage() Error: [currentIndex + 1] cant be \'<\' or \'<\' .");
                        }
                        else // it is a openning Tag
                        {
                            state = ParserStageState.FindingOppeningTag;
                            tempIndex = currentIndex;
                        }
                    }
                }
                else if (sourcArray[currentIndex] == CloseAngleBracket) // currentIndex = '>'
                {
                    if (state == ParserStageState.FindingOppeningTag)
                    {
                        // successfuly found a oppening Tag:
                        char[] subArray = sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                        string tag = new string(subArray);

                        // add the current founded Tag to the List:
                        listOfTags.Add(new XmlTagInfo(sourcArray, tag, tempIndex));
                        tempIndex = -1;
                        state = ParserStageState.None;
                    }
                    else if (state == ParserStageState.FindingClosingTag)
                    {
                        // successfuly found a clossing Tag:
                        char[] subArray = sourcArray.SubArray(tempIndex + 2, currentIndex - 1);
                        string tag = new string(subArray);
                        // found the current none complete Tag in the list and complete it:
                        bool tagIsFound = false;
                        foreach (XmlTagInfo tagInfo in listOfTags)
                        {
                            if (tagInfo.IsComplete()) continue;
                            else if (tagInfo.Tag.Equals(tag))
                            {
                                tagInfo.SetEndTagIndex(currentIndex);
                                tempIndex = -1;
                                state = ParserStageState.None;
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
                    else if (state == ParserStageState.None)
                    {
                        throw new Exception($"XmlParser.CompleteParserStage() Error: parser should be in an state before reach \'>\' character. ");
                    }
                }

                // Debugging:
                // check processError at end of each iteration: 
                //if(parserStageError == 0)
                //{
                //    Console.WriteLine($"parsser success! currentIndex: {currentIndex}   currentChar: \'{sourcArray[currentIndex]}\'");
                //}
                //else
                //{
                //    Console.WriteLine($"parsser Error({parserStageError})! currentIndex: {currentIndex}   currentChar: \'{sourcArray[currentIndex]}\'");
                //}
            }
        }
        // process the charArray until found most wanted wantedTag and insert founded tags to listOfTags:        
        private void SlightParserStage(string wantedTag)
        {
            bool continueParsing = true;
            int tempIndex = -1; // -1 means nothing
            ParserStageState state = ParserStageState.None;

            for (int currentIndex = 0; (currentIndex < sourcArray.Length && continueParsing); currentIndex++)
            {
                if (sourcArray[currentIndex] == OpenAngleBracket) // currentIndex = '<'
                {
                    // if the last iteration is '<' so there is an error:                  
                    if (currentIndex == sourcArray.Length - 1)
                    {
                        throw new Exception($"XmlParser.CompleteParserStage() Error: Last index cant be \'<\' .");
                    }
                    else
                    {
                        if (sourcArray[currentIndex + 1] == SlashSymbol) // it is a clossing Tag
                        {
                            state = ParserStageState.FindingClosingTag;
                            tempIndex = currentIndex;
                        }
                        else if (sourcArray[currentIndex + 1] == CloseAngleBracket || sourcArray[currentIndex + 1] == OpenAngleBracket)
                        {
                            throw new Exception($"XmlParser.CompleteParserStage() Error: [currentIndex + 1] cant be \'<\' or \'<\' .");
                        }
                        else // it is a openning Tag
                        {
                            state = ParserStageState.FindingOppeningTag;
                            tempIndex = currentIndex;
                        }
                    }
                }
                else if (sourcArray[currentIndex] == CloseAngleBracket) // currentIndex = '>'
                {
                    if (state == ParserStageState.FindingOppeningTag)
                    {
                        // successfuly found a oppening Tag:
                        char[] subArray = sourcArray.SubArray(tempIndex + 1, currentIndex - 1);
                        string tag = new string(subArray);

                        // add the current founded Tag to the List:
                        listOfTags.Add(new XmlTagInfo(sourcArray, tag, tempIndex));
                        tempIndex = -1;
                        state = ParserStageState.None;
                    }
                    else if (state == ParserStageState.FindingClosingTag)
                    {
                        // successfuly found a clossing Tag:
                        char[] subArray = sourcArray.SubArray(tempIndex + 2, currentIndex - 1);
                        string tag = new string(subArray);
                        // found the current none complete Tag in the list and complete it:
                        bool tagIsFound = false;
                        foreach (XmlTagInfo tagInfo in listOfTags)
                        {
                            if (tagInfo.IsComplete()) continue;
                            else if (tagInfo.Tag.Equals(tag))
                            {
                                tagInfo.SetEndTagIndex(currentIndex);                         
                                tempIndex = -1;
                                state = ParserStageState.None;
                                tagIsFound = true;

                                // check if current founded closing Tag is equal to wnatedTag then stop parsing:
                                if (wantedTag.Equals(tag))continueParsing = false;
                                break;
                            }
                        }
                        // if nothing found in previus foreach loop so there is an error:
                        if (tagIsFound == false)
                        {
                            throw new Exception($"XmlParser.CompleteParserStage() Error: Cant found any matched Tag with {tag} .");
                        }
                    }
                    else if (state == ParserStageState.None)
                    {
                        throw new Exception($"XmlParser.CompleteParserStage() Error: parser should be in an state before reach \'>\' character. ");
                    }
                }

                // Debugging:
                // check processError at end of each iteration: 
                //if(parserStageError == 0)
                //{
                //    Console.WriteLine($"parsser success! currentIndex: {currentIndex}   currentChar: \'{sourcArray[currentIndex]}\'");
                //}
                //else
                //{
                //    Console.WriteLine($"parsser Error({parserStageError})! currentIndex: {currentIndex}   currentChar: \'{sourcArray[currentIndex]}\'");
                //}
            }
        }
        // set nodes relationship together:
        private void ParrentingStage()
        {
            // the first node is always the root:
            rootNode = listOfTags[0];

            for (int i = 0; i < listOfTags.Count; i++)
            {
                XmlTagInfo currentNode = listOfTags[i];
                for (int j = i + 1; j < listOfTags.Count; j++)
                {
                    XmlTagInfo currentfoundedNode = listOfTags[j];
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
            foreach (XmlTagInfo currentNode in listOfTags)
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
            foreach (XmlTagInfo currentNode in listOfTags)
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
        public static string ConvertToXmlFormat(string content, string tag , string seprator = " ")
        {
            return $"<{tag}>{seprator}{content}{seprator}</{tag}>";
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
        private XmlTagInfo parrentNode = null;
        private List<XmlTagInfo> listOfChildren = new List<XmlTagInfo>();

        public string Tag { get { return tag; } }
        public int StartTagIndex { get { return startTagIndex; } }
        public int EndTagIndex { get { return endTagIndex; } }
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
        public override string ToString()
        {
            return $" (XmlTagInfo object) => Tag:({tag})  startIndex:({startTagIndex})  endIndex:({endTagIndex})";
        }
        public string GetContent()
        {
            return new string(sourcArray.SubArray(startTagIndex + tag.Length + 2, endTagIndex - tag.Length - 3));
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
    }

}

