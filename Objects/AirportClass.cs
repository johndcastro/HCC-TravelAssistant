using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot.Objects
{

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class airport
        {

            private string nameField;

            private string shortcodeField;

            private string cityField;

            private string stateField;

            private double latitudeField;

            private double longitudeField;

            private sbyte utcField;

            private string dstField;

            private bool precheckField;

            private airportCheckpoints checkpointsField;

            /// <remarks/>
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string shortcode
            {
                get
                {
                    return this.shortcodeField;
                }
                set
                {
                    this.shortcodeField = value;
                }
            }

            /// <remarks/>
            public string city
            {
                get
                {
                    return this.cityField;
                }
                set
                {
                    this.cityField = value;
                }
            }

            /// <remarks/>
            public string state
            {
                get
                {
                    return this.stateField;
                }
                set
                {
                    this.stateField = value;
                }
            }

            /// <remarks/>
            public double latitude
            {
                get
                {
                    return this.latitudeField;
                }
                set
                {
                    this.latitudeField = value;
                }
            }

            /// <remarks/>
            public double longitude
            {
                get
                {
                    return this.longitudeField;
                }
                set
                {
                    this.longitudeField = value;
                }
            }

            /// <remarks/>
            public sbyte utc
            {
                get
                {
                    return this.utcField;
                }
                set
                {
                    this.utcField = value;
                }
            }

            /// <remarks/>
            public string dst
            {
                get
                {
                    return this.dstField;
                }
                set
                {
                    this.dstField = value;
                }
            }

            /// <remarks/>
            public bool precheck
            {
                get
                {
                    return this.precheckField;
                }
                set
                {
                    this.precheckField = value;
                }
            }

            /// <remarks/>
            public airportCheckpoints checkpoints
            {
                get
                {
                    return this.checkpointsField;
                }
                set
                {
                    this.checkpointsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class airportCheckpoints
        {

            private airportCheckpointsCheckpoint checkpointField;

            /// <remarks/>
            public airportCheckpointsCheckpoint checkpoint
            {
                get
                {
                    return this.checkpointField;
                }
                set
                {
                    this.checkpointField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class airportCheckpointsCheckpoint
        {

            private byte idField;

            private string longnameField;

            private string shortnameField;

            /// <remarks/>
            public byte id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public string longname
            {
                get
                {
                    return this.longnameField;
                }
                set
                {
                    this.longnameField = value;
                }
            }

            /// <remarks/>
            public string shortname
            {
                get
                {
                    return this.shortnameField;
                }
                set
                {
                    this.shortnameField = value;
                }
            }
        }



    }
