import { useState } from 'react';
import axios from 'axios';

const SynonymSearch = () => {
	const [word, setWord] = useState('');
	const [results, setResults] = useState([]);
	const [error, setError] = useState('');

	const search = async () => {
		setError('');
		axios
			.get(`${process.env.REACT_APP_API_URL}/synonyms/${word}`)
			.then(res => {
				setResults(res.data);
			})
			.catch(err => {
				console.error('Error fetching synonyms:', err);
				setError('Failed to fetch synonyms. Please try again.');
			});
	};

	return (
		<div className='synonym-search'>
			<h2>Find Synonyms</h2>
			<div className='form-row'>
				<input value={word} onChange={e => setWord(e.target.value)} placeholder='Word' />
			</div>
			<div className='form-button'>
				<button onClick={search}>Search</button>
			</div>

			{error && <div className='error-message'>{error}</div>}

			<ul>
				{results.map(s => (
					<li key={s.id}>{s.text}</li>
				))}
			</ul>
		</div>
	);
};

export default SynonymSearch;