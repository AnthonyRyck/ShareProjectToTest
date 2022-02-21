import Fan from '../Models/Fan'

export default class AccessData {
	
	BASE_URL_API_FAN:string

	constructor()
	{
		this.BASE_URL_API_FAN = "https://fandemo-wasm.ctrl-alt-suppr.dev/FansApi/";
	}

	


	public async GetAllFans(): Promise<Array<Fan>>
	{
		return await this.GetHttp<Array<Fan>>(this.BASE_URL_API_FAN);
	}

	public async GetFan(id: number): Promise<Fan>
	{
		return await this.GetHttp(this.BASE_URL_API_FAN + id.toString());
	}

	public async AddFan(name: string): Promise<Fan>
	{
		let urlAdd = this.BASE_URL_API_FAN + "newfan/" + name;
		return await this.PostHttpWithResult<Fan>(urlAdd, name);
	}

	public async RemoveFan(fanToDelete: Fan): Promise<void>
	{
		let url = this.BASE_URL_API_FAN + fanToDelete.Id.toString();
		await this.DeleteHttp(url, fanToDelete.Id);
	}

	public async AddClick(idFan: number): Promise<number>
	{
		let urlAddClick = this.BASE_URL_API_FAN + idFan.toString();
		return await this.PostWithResult<number>(urlAddClick);
	}


	private async GetHttp<T>(url: string): Promise<T>
	{
		let response = await fetch(url);
		const body = await response.json();
		return body;
	}

	private async PostHttp(url: string, info: any): Promise<void>
	{
		let request = new Request(url,
			{
			  method: "post",
			  body: JSON.stringify({info})
			});

		await fetch(request);
	}

	private async PostWithResult<T>(url: string): Promise<T>
	{
		let request = new Request(url,
			{
			  method: "post"
			});

		let result = await fetch(request);
		return result.json();
	}

	private async PostHttpWithResult<T>(url: string, info: any): Promise<T>
	{
		let request = new Request(url,
			{
			  method: "post",
			  body: JSON.stringify({info})
			});

		let result = await fetch(request);
		return result.json();
	}

	private async DeleteHttp(url: string, info: any): Promise<void>
	{
		let request = new Request(url,
			{
			  method: "deletee",
			  body: JSON.stringify({info})
			});

		await fetch(request);
	}
}