import React, { Component, MouseEventHandler } from 'react';
import FanComponent from '../components/FanComponent';
import Navbar from '../components/Navbar/Navbar';
import '../css/fanapp.css';
import AccessData from '../data/FakeAccessData';

type FanClubState = {
	viewModel: AccessData
}

export default class FanClub extends React.Component<{}, FanClubState> {
	
	render() {
	return (

		<div className="page">
        	<div className="sidebar">
        	    <Navbar />
        	</div>
        	<div>Coucou de la FanClub page</div>

			<div className="fanClub">
				fan club page :/
				{/* { 
					(await this.state.viewModel.GetAllFans()).map((fan, index) =>
					{
						<FanComponent nom="{fan.nom}"
						id="{fan.id}"
						nombreClick="{fan.nombreClick}"
								   />
								//    onClickCounter={}
								//   onClickToFanPage={}
							}) 
						}; */}
			</div>
		</div>
	);
	}
}