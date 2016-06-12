--*****************************************************
--�ַ�������
--*****************************************************

function Split(szFullString, szSeparator)
local nFindStartIndex = 1
local nSplitIndex = 1
local nSplitArray = {}
while true do
   local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)
   if not nFindLastIndex then
		diviseString = string.sub(szFullString, nFindStartIndex, string.len(szFullString))
		if  diviseString ~= "" then
			nSplitArray[nSplitIndex] = diviseString
		end
	break
   end
   nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)
   nFindStartIndex = nFindLastIndex + string.len(szSeparator)
   nSplitIndex = nSplitIndex + 1
end
	return nSplitArray
end
