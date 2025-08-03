import './App.css';
import SynonymForm from './components/SynonymForm';
import SynonymSearch from './components/SynonymSearch';

function App() {
	return (
		<div className='app-wrapper'>
			<div className='App'>
				<h1>Synonyms Tool</h1>
				<div className='form-container'>
					<SynonymForm />
					<hr />
					<SynonymSearch />
				</div>
			</div>
			<footer className='footer'>© {new Date().getFullYear()} Džejlan Šabić</footer>
		</div>
	);
}

export default App;
