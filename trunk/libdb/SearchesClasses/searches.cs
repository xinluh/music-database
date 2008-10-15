using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Data;

namespace libdb
{
    public class ArtistSearch : SearchBase
    {
        public enum Fields
        {
            [Des("id")]                 ID        ,
            [Des("type")]               Type      ,
            [Des("last_name")]          LastName  ,
            [Des("first_name")]         FirstName ,
            [Des("fullname")]           FullName  ,
            [Des("type_id")]            TypeID    ,
        }

        public enum TypeCategory
        {
            [Des("{0} = 1")]               Composers = 1,
            [Des("{0} > 6 AND {0} != 29")] SingleArtists = 2,
            [Des("{0} = 6")]               Conductors = 4,
            [Des("{0} >= 3 AND {0} <= 5")] Ensembles = 8,
            [Des("{0} = 2 OR {0} = 29")]   Other = 16,
        }

        protected override string table { get { return "vwArtist"; } }
        protected override string orderby { get { return ""; } }

        public ArtistSearch(params Fields[] fields)
        {
            SetFieldToSearch(fields);
        }

        public void SetFieldToSearch(Fields[] fields) {set_field_to_search(fields.Cast<object>()); }
        /// <summary>
        /// Add a filter to search. Filter string can be like " = 1", then the column/field name is automatically
        /// inserted in the front; or it can also be a string.format string, like "{0} = 1 or {0} = 15", then
        /// string.format will be called to replace all the "{0}" with the column/field name.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="filterstring"></param>
        public void AddFilter(Fields f, string filterstring) { add_filter(f, filterstring); }
        /// <summary>
        /// Search a text field for all of the words (i.e. space-separated) in the "phrases" parameter.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="phrases"></param>
        public void AddWordFilter(Fields f, string phrases) { add_words_filter(f, phrases); }
        /// <summary>
        /// Clear all filter associated with a field/column
        /// </summary>
        /// <param name="f"></param>
        public void ClearFilters(Fields f) { filters.Remove(f); }

        public void AddTypeToSearch(TypeCategory type)
        {
            AddFilter(Fields.TypeID, enum_to_des(type)[0]);
        }

        public void ClearTypesToSearch()
        {
            ClearFilters(Fields.TypeID);
        }

    }
public class PieceSearch : SearchBase
    {
        public enum Fields
        {
            [Des("id")]                 ID        ,
            [Des("parent_piece_id")]    ParentPieceID,
            [Des("name")]               Name  ,
            [Des("connector")]          Connector,
            [Des("composer_id")]        ComposerID,
            [Des("genre_id")]           GenreID,
        }
        protected override string table { get { return "tblPiece"; } }
        protected override string orderby { get { return ""; } }

        public PieceSearch(params Fields[] fields)
        {
            SetFieldToSearch(fields);
        }

        public void SetFieldToSearch(Fields[] fields) {set_field_to_search(fields.Cast<object>()); }
        /// <summary>
        /// Add a filter to search. Filter string can be like " = 1", then the column/field name is automatically
        /// inserted in the front; or it can also be a string.format string, like "{0} = 1 or {0} = 15", then
        /// string.format will be called to replace all the "{0}" with the column/field name.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="filterstring"></param>
        public void AddFilter(Fields f, string filterstring) { add_filter(f, filterstring); }
        /// <summary>
        /// Search a text field for all of the words (i.e. space-separated) in the "phrases" parameter.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="phrases"></param>
        public void AddWordFilter(Fields f, string phrases) { add_words_filter(f, phrases); }    
        /// <summary>
        /// Clear all filter associated with a field/column
        /// </summary>
        /// <param name="f"></param>
        public void ClearFilters(Fields f) { filters.Remove(f); }



    }

public class GenreSearch : SearchBase
    {
        public enum Fields
        {
                [Des("id")]                 ID        ,
                [Des("name")]               Name  ,
                [Des("composer_id")]        ComposerID,
	
        }
        protected override string table { get { return "vwGenreComposer"; } }
        protected override string orderby { get { return ""; } }

        public GenreSearch(params Fields[] fields)
        {
            SetFieldToSearch(fields);
        }

        public void SetFieldToSearch(Fields[] fields) { set_field_to_search(fields.Cast<object>()); }
        /// <summary>
        /// Add a filter to search. Filter string can be like " = 1", then the column/field name is automatically
        /// inserted in the front; or it can also be a string.format string, like "{0} = 1 or {0} = 15", then
        /// string.format will be called to replace all the "{0}" with the column/field name.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="filterstring"></param>
        public void AddFilter(Fields f, string filterstring) { add_filter(f, filterstring); }
        /// <summary>
        /// Search a text field for all of the words (i.e. space-separated) in the "phrases" parameter.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="phrases"></param>
        public void AddWordFilter(Fields f, string phrases) { add_words_filter(f, phrases); }
        /// <summary>
        /// Clear all filter associated with a field/column
        /// </summary>
        /// <param name="f"></param>
        public void ClearFilters(Fields f) { filters.Remove(f); }



}
}
