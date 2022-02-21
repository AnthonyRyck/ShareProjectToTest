//import * as WeatherForecasts from '../ViewModels';
import * as FanClubViewModel from "../ViewModels/FanClubViewModel";

// The top-level state object
export interface ApplicationState {
	fanclubViewModel: FanClubViewModel.FanClubState;
    // counter: Counter.CounterState | undefined;
    // weatherForecasts: WeatherForecasts.WeatherForecastsState | undefined;
}

// Chaque fois qu'une action est expédiée, Redux mettra à jour chaque propriété d'état d'application de niveau supérieur à l'aide de
// le réducteur avec le nom correspondant.Il est important que les noms correspondent exactement et que le réducteur
// agit sur le type de propriété d'application correspondant.
export const reducers = {
    // counter: Counter.reducer,
    // weatherForecasts: WeatherForecasts.reducer
	fanclubViewModel: FanClubViewModel.reducer
};

// Ce type peut être utilisé comme indice d'action des créateurs d'action afin que ses paramètres «Dispatch» et «GetState» soient
// correctement tapé pour correspondre à votre magasin.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}
