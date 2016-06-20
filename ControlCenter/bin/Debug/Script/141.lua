require("LuaLib.Split")
--name oprations ={ name, type, datatype, data, port, time}

local commands = {[1]  = { operationType ="TCP", operationDataType ="Character", operationData ="play",Setting = {ip = "127.0.0.1",port ="3000"},operationTime ="100"}
};

StayAlive(300);

local function ChangeStringToTable(data)
	local length = string.len(data);
	local command = {};
	for i = 1,length do
	char = string.sub(data, i, i);
	local data = string.byte(char)
	table.insert(command,i,data);
	end
	return command;
end

local function ChangeHexStringToTable(data)
	local length = string.len(data);
	local command = {};
	j = 1;
	for i = 1,length, 2 do
		temp = "";
		if i + 1 <= length then
			temp = string.sub(data, i, i + 1);
		else
			temp = string.sub(data, i,i);
		end

		local data = tonumber(temp, 16);
		table.insert(command,j,data);
		j = j + 1;
	end
	return command;
end


for operationName,  operation in ipairs(commands) do

	delayTime = 200;
	if operation["operationTime"] ~= nil then
		if tonumber(operation["operationTime"]) > delayTime then
			delayTime = tonumber(operation["operationTime"]);
		end
	end
	Sleep(delayTime);

	command = nil;
	if operation["operationData"] ~= nil then
		if string.lower(operation["operationDataType"]) == "hex" then
			command = ChangeHexStringToTable(operation["operationData"]);
		else
			command = ChangeStringToTable(operation["operationData"]);
		end
	end

	Setting = operation["Setting"];
	if string.lower(operation["operationType"]) == "com" then
		targetport = Setting["comNumber"];
		comBaudrate = tonumber(Setting["baudRate"]);
		comDataBit = tonumber(Setting["dataBit"]);
		comStopBit = tonumber(Setting["stopBit"]);
		comParity = Setting["parity"];

		SendComData(targetport,comBaudrate, comDataBit, comStopBit,comParity, command);
	elseif string.lower(operation["operationType"]) == "tcp" then
		ip = operation["Setting"]["ip"];
		port = tonumber(operation["Setting"]["port"]);

		SendTcpData(ip, port, command);


	elseif string.lower(operation["operationType"]) == "udp" then
		ip = operation["Setting"]["ip"];
		port = tonumber(operation["Setting"]["port"]);

		SendUdpData(ip, port, command);
	end

end

collectgarbage("collect")

return 0;

