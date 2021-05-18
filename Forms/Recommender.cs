using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyAPI.Web;

namespace reAudioPlayerML
{
    public partial class Recommender : Form
    {
        private SpotifyClient client;
        private FullTrack trackAv;
        private string[] genresAv;
        private FullArtist[] artistsAv;
        private Search.Spotify spotify;

        private MediaPlayer player;

        private List<FullArtist> artists = new List<FullArtist>();
        private FullTrack track;
        private List<string> genres = new List<string>();

        private List<SimpleTrack> recommendations = new List<SimpleTrack>();

        public Recommender(Search.Spotify spotify, MediaPlayer player, FullTrack track)
        {
            this.track = this.trackAv = track;
            this.spotify = spotify;
            this.client = spotify.client;
            this.player = player;

            InitializeComponent();

            updateNodeToMatchArray("track", new string[] { track.Name });

            genresAv = getAssociatedGenresOfTrack(track);

            cmbArtists.Items.AddRange(artistsAv.Select(x => x.Name).ToArray());
            cmbGenres.Items.AddRange(genresAv);

            ctxRecommendation.Items["preview"].Click += Recommender_Click;
            ctxRecommendation.Items["addToPlaylist"].Click += Recommender_Click1;
            ctxRecommendation.Items["open"].Click += Recommender_Click2;
            cmbPlaylists = (ctxRecommendation.Items["addToPlaylist"] as ToolStripMenuItem).DropDownItems["playlists"] as ToolStripComboBox;

            foreach (var it in spotify.playlists.Items)
            {
                cmbPlaylists.Items.Add(it);
            }
        }

        private void Recommender_Click2(object sender, EventArgs e)
        {

            spotify.openOnSpotify(recommendations[listView1.SelectedIndices[0]]);
        }

        private void Recommender_Click1(object sender, EventArgs e)
        {
            ToolStripMenuItem parent = sender as ToolStripMenuItem;
            ToolStripComboBox item = parent.DropDownItems[0] as ToolStripComboBox;
            
            if (item.Text == "")
            {
                return;
            }

            spotify.addToPlaylist(recommendations[listView1.SelectedIndices[0]].Uri, item.Text);

            MessageBox.Show("Song added!");
        }

        ToolStripComboBox cmbPlaylists;

        private void Recommender_Click(object sender, EventArgs e)
        {
            var id = recommendations[listView1.SelectedIndices[0]].Id;
            var ft = client.Tracks.Get(id).Result;
            new Search.SpotifyPreview(player, ft);
        }

        private string[] getAssociatedGenresOfTrack(FullTrack track)
        {
            var artists = track.Artists.Select(x => x.Id).ToList();

            var al = client.Artists.GetSeveral(new ArtistsRequest(artists)).Result;
            this.artistsAv = al.Artists.ToArray();
            var genres = al.Artists.SelectMany(x => x.Genres).ToList();

            var ar = client.Albums.Get(track.Album.Id).Result;
            genres.AddRange(ar.Genres);

            return genres.ToArray();
        }

        private void Recommender_Load(object sender, EventArgs e)
        {
            
        }

        private void btnAddArtist_Click(object sender, EventArgs e)
        {
            artists.Add(artistsAv.Where(x => x.Name == cmbArtists.Text).FirstOrDefault());
            updateNodeToMatchArray("artists", artists.Select(x => x.Name).ToArray());   
        }

        private void updateNodeToMatchArray(string child, string[] array)
        {
            tvSeeds.BeginUpdate();
            tvSeeds.Nodes["root"].Nodes[child].Nodes.Clear();
            foreach (var element in array)
            {
                tvSeeds.Nodes["root"].Nodes[child].Nodes.Add(element);
            }

            tvSeeds.EndUpdate();
            tvSeeds.ExpandAll();
        }

        private void btnAddGenre_Click(object sender, EventArgs e)
        {
            genres.Add(cmbGenres.Text);
            updateNodeToMatchArray("genres", genres.ToArray());
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            RecommendationsRequest rr = new RecommendationsRequest();
            rr.Limit = 100;
            var features = spotify.getFeatures(track.Id);
            rr.Target.Add("key", features.Key.ToString());
            rr.Target.Add("tempo", features.Tempo.ToString());

            rr.SeedTracks.Add(track.Id);

            foreach (var artist in artists)
            {
                rr.SeedArtists.Add(artist.Id);
            }

            foreach (var genre in genres)
            {
                rr.SeedGenres.Add(genre);
            }

            var recs = client.Browse.GetRecommendations(rr).Result;

            recommendations = new List<SimpleTrack>(recs.Tracks);

            listView1.Items.Clear();

            foreach (var rec in recs.Tracks)
            {
                listView1.Items.Add(rec.Name);
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(String.Join(", ", rec.Artists.Select(x => x.Name)));
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(rec.PreviewUrl);
            }
        }
    }
}
