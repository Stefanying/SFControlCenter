require("LuaLib.Split")
--name oprations ={ name, type, datatype, data, port, time}

commands = {#commands};

DontKillMe(300);

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
		PrintToLog(temp)
		local data = tonumber(temp, 16);
		table.insert(command,j,data);
		j = j + 1;
	end
	return command;
end

for name, operations in pairs(commands) do

	for operationName,operation in pairs(operations) do

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

		if string.lower(operation["operationType"]) == "com" then
			targetport = operation["operationPort"];
			SendComData(targetport, command);

		elseif string.lower(operation["operationType"]) == "network" then
			ip_port = operation["operationPort"];
			ip_port_data = Split(ip_port, ":")
			ip = ip_port_data[1];
			port = tonumber(ip_port_data[2]);

			SendTcpData(ip, port, command);
		end

	end

end
