import Fan from '../Models/Fan'
import axios from "axios";

	
const apiClient = axios.create({
	baseURL: "https://fandemo-wasm.ctrl-alt-suppr.dev/FansApi/",
	headers: {
	  "Content-type": "application/json"
	},
});

const GetAllFans = async () =>
{
	const response = await apiClient.get<Fan[]>("");
  	return response.data;
}

const GetFan = async (id: number) =>
{
	return await apiClient.get<Fan>(id.toString());
}

const AddFan = async(name: string) =>
{
	return await apiClient.post<any>("newfan/${name}");
}

// const RemoveFan(fanToDelete: Fan): Promise<void>
// {
// 	let url = this.BASE_URL_API_FAN + fanToDelete.Id.toString();
// 	await this.DeleteHttp(url, fanToDelete.Id);
// }

// const AddClick(idFan: number): Promise<number>
// {
// 	let urlAddClick = this.BASE_URL_API_FAN + idFan.toString();
// 	return await this.PostWithResult<number>(urlAddClick);
// }

const AccessDataService = {
	GetAllFans,
	GetFan,
	AddFan
  }
  
  export default AccessDataService;