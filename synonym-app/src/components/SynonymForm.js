import { useState } from 'react';
import axios from 'axios';

const SynonymForm = () => {
	const [word, setWord] = useState('');
	const [synonyms, setSynonyms] = useState('');
	const [error, setError] = useState('');
	const [success, setSuccess] = useState(false);

	const handleSubmit = e => {
		e.preventDefault();
		setError('');

		const synonymList = synonyms
			.split(',')
			.map(s => s.trim())
			.filter(s => s.length > 0);

		axios
			.post(`${process.env.REACT_APP_API_URL}/synonyms`, {
				word,
				synonym: synonymList,
			})
			.then(() => {
				setWord('');
				setSynonyms('');
				setSuccess(true);
			})
			.catch(err => {
				console.error('Error adding synonym:', err);
				setSuccess(false);
				if (err.response?.data?.message) {
					setError(err.response.data.message);
				} else {
					setError('An unexpected error occurred.');
				}
			});
	};

	return (
		<form onSubmit={handleSubmit}>
			<h2>Add Synonyms</h2>

			{error && <div style={{ color: 'red', marginBottom: '1rem' }}>{error}</div>}
			{success && <div style={{ color: 'green', marginBottom: '1rem' }}>Synonyms added successfully!</div>}
			<div className='form-row'>
				<label>Word:</label>
				<input value={word} onChange={e => setWord(e.target.value)} placeholder='Word' />
			</div>

			<div className='form-row'>
				<label>Synonyms:</label>
				<input value={synonyms} onChange={e => setSynonyms(e.target.value)} placeholder='Separated by commas' />
			</div>

			<div className='form-button'>
				<button type='submit'>Add Synonyms</button>
			</div>
		</form>
	);
};

export default SynonymForm;
