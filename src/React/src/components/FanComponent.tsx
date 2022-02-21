import React, { Component, MouseEventHandler } from 'react';
import './FanComponent.css';

interface FanComponentProps {
  id: string;
  nom: string;
  nombreClick: string;
  // onClickCounter: MouseEventHandler;
  // onClickToFanPage: MouseEventHandler;
}

export default class FanComponent extends Component<FanComponentProps> {
  render() {
    const { id, nom, nombreClick,  } = this.props;
    // onClickCounter, onClickToFanPage
    return (
      <>
        <div id="openFanPage" className="fan grow" >
        {/* onClick={onClickToFanPage} */}
			<div>
				<div>ID : {id}</div>
				<div id="iam">Nom : {nom}</div>
				<div>Nombre de click : {nombreClick}</div>
			</div>

			<button >
      {/* onClick={onClickCounter} */}
				Click sur Moi
			</button>
		</div>
      </>
    );
  }
}